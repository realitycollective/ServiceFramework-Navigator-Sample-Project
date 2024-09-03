using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealityCollective.Utilities.Logging;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

#region Permission Enumerations
public enum PermissionType
    {
        /// <summary>
        /// Request Camera access for the device.
        /// </summary>
        Camera,

        /// <summary>
        /// Request Microphone access for the device.
        /// </summary>
        Microphone,

        /// <summary>
        /// Request Fine Location access for the device.
        /// </summary>
        FineLocation,

        /// <summary>
        /// Request Coarse Location access for the device.
        /// </summary>
        CoarseLocation,

        ExternalStorageRead,

        ExternalStorageWrite
    }

    public enum PermissionStatus
    {
        /// <summary>
        /// None or Unknown status.
        /// </summary>
        None,

        /// <summary>
        /// Has the permission been granted for the application?
        /// </summary>
        Granted,

        /// <summary>
        /// Has the user denied the permission, but has not selected "Do not ask again."
        /// </summary>
        Denied,

        /// <summary>
        /// User has denied the permission and selected "Do not ask again."
        /// </summary>
        DeniedAndCannotRequest
    }
    #endregion Permission Enumerations

    /// <summary>
    /// Helper class for validating required permissions for a platform.
    /// </summary>
    public class PermissionValidator : MonoBehaviour
    {
        #region Editor Properties
        [SerializeField]
        private bool requestPermissionsOnStart = true;

        [SerializeField]
        private PermissionType[] requiredPermissions = null;
        #endregion Editor Properties

        #region Private Properties
        private static readonly Dictionary<PermissionType, string> PlatformPermissionMap = new Dictionary<PermissionType, string>()
#if UNITY_ANDROID
      {
        /// Android permission documentation: https://developer.android.com/reference/android/Manifest.permission
        { PermissionType.Camera, Permission.Camera },
        { PermissionType.Microphone, Permission.Microphone },
        { PermissionType.FineLocation, Permission.FineLocation },
        { PermissionType.CoarseLocation, Permission.CoarseLocation },
        { PermissionType.ExternalStorageRead, Permission.ExternalStorageRead },
        { PermissionType.ExternalStorageWrite, Permission.ExternalStorageWrite }
      };
#else
            ;
#endif
        #endregion Private Properties

        #region MonoBehaviours
        async void Start()
        {
            if (requestPermissionsOnStart)
            {
                await RequestPermissionsAsync();
            }
        }

        public async Task RequestPermissionsAsync()
        {
            foreach (var permission in requiredPermissions)
            {
                if (!HasPermission(permission))
                {
                    await RequestPermissionAsync(permission);
                }
            }
        }
        #endregion MonoBehaviours

        #region Public Methods
        /// <summary>
        /// Request access for a given permission async.
        /// </summary>
        /// <param name="permission">The <see cref="PermissionType"/> to request.</param>
        /// <returns>The response status for the given permission.</returns>
        public static async Task<PermissionStatus> RequestPermissionAsync(PermissionType permission)
        {
            PermissionStatus permissionStatusResult = PermissionStatus.None;
            if (PlatformPermissionMap.ContainsKey(permission))
            {
                permissionStatusResult = await RequestPermissionAsync(PlatformPermissionMap[permission]);
            }
            CheckAdditionalPermissionAccess(permission);
            return permissionStatusResult;
        }

        /// <summary>
        /// Request access for a given permission.
        /// </summary>
        /// <param name="permission">The <see cref="PermissionType"/> to request.</param>
        /// <param name="callback">Response status for the given permission.</param>
        public static void RequestPermission(PermissionType permission, Action<PermissionStatus> callback)
        {
            if (PlatformPermissionMap.ContainsKey(permission))
            {
                RequestPermission(PlatformPermissionMap[permission], callback);
            }
            CheckAdditionalPermissionAccess(permission);
        }

        /// <summary>
        /// Has the user granted the selected permission.
        /// </summary>
        /// <param name="permission">The <see cref="PermissionType"/> to validate.</param>
        /// <returns>True if the User has granted the permission for this application.</returns>
        public static bool HasPermission(PermissionType permission)
        {
            var granted = true;
            if (PlatformPermissionMap.ContainsKey(permission))
            {
                granted = HasPermission(PlatformPermissionMap[permission]);
            }
            return granted;
        }
        #endregion Public Methods

        #region Private Methods
        private static bool HasPermission(string permissionName)
        {
            var granted = true;
#if UNITY_ANDROID
            granted = Permission.HasUserAuthorizedPermission(permissionName);
#endif
            return granted;
        }

        private static Task<PermissionStatus> RequestPermissionAsync(string permissionName)
        {
            var t = new TaskCompletionSource<PermissionStatus>();
            RequestPermission(permissionName, (status) => t.TrySetResult(status));
            return t.Task;
        }

        private static void RequestPermission(string permissionName, Action<PermissionStatus> callback)
        {
            if (HasPermission(permissionName))
            {
                callback.Invoke(PermissionStatus.Granted);
                return;
            }

#if UNITY_ANDROID
            var callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += status => callback.Invoke(PermissionStatus.Granted);
            callbacks.PermissionDenied += status => callback.Invoke(PermissionStatus.Denied);
#pragma warning disable CS0618 // Type or member is obsolete
            callbacks.PermissionDeniedAndDontAskAgain += status => callback.Invoke(PermissionStatus.DeniedAndCannotRequest);
#pragma warning restore CS0618 // Type or member is obsolete
            Permission.RequestUserPermission(permissionName, callbacks);
#endif
        }

        private static void CheckAdditionalPermissionAccess(PermissionType permission)
        {
            switch (permission)
            {
                case PermissionType.FineLocation:
                case PermissionType.CoarseLocation:
                    ValidateUnityLocationAccess();
                    break;
                default:
                    break;
            }
        }

        private static void ValidateUnityLocationAccess()
        {
#if UNITY_ANDROID
            // First, check if user has location service enabled
            if (!Input.location.isEnabledByUser)
            {
                StaticLogger.LogWarning("Android Location not enabled");
            }
#elif UNITY_IOS
            if (!Input.location.isEnabledByUser)
            {
                StaticLogger.LogWarning("IOS Location not enabled");
            }
#endif
        }
        #endregion Private Methods
    }
