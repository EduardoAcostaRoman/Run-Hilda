#if EXTERNAL_FRAME_PACING_CODE

#include <stdarg.h>
#include <android/log.h>
#include "swappy/swappyGL.h"
#include "swappy/swappyGL_extra.h"
#include "swappy/swappyVk.h"
#include "UnitySwappyWrapper.h"

#define UNITY_MIN_SWAPPY_LOG_PRIORITY ANDROID_LOG_FATAL

extern "C" int __real___android_log_print(int prio, const char* tag, const char* fmt, ...);

static void SwappyWrapperSetFunctionProvider(const Unity::SwappyVkFunctionProvider* pSwappyVkFunctionProvider)
{
    static SwappyVkFunctionProvider provider;
    provider.init = pSwappyVkFunctionProvider->init;
    provider.getProcAddr = pSwappyVkFunctionProvider->getProcAddr;
    provider.close = pSwappyVkFunctionProvider->close;
    SwappyVk_setFunctionProvider(&provider);
}


UNITY_SWAPPYWRAPPER_EXPORT bool UnitySwappyWrapperInit(Unity::SwappyCommonFunctions* commonfn, Unity::SwappyGLFunctions* glfn, Unity::SwappyVKFunctions* vkfn)
{
    commonfn->version = Swappy_version;
    commonfn->setThreadFunctions = Swappy_setThreadFunctions;
    commonfn->versionString = Swappy_versionString;

    glfn->init = SwappyGL_init;
    glfn->isEnabled = SwappyGL_isEnabled;
    glfn->destroy = SwappyGL_destroy;
    glfn->setWindow = SwappyGL_setWindow;
    glfn->swap = SwappyGL_swap;
    glfn->setUseAffinity = SwappyGL_setUseAffinity;
    glfn->setSwapIntervalNS = SwappyGL_setSwapIntervalNS;
    glfn->setFenceTimeoutNS = SwappyGL_setFenceTimeoutNS;
    glfn->getRefreshPeriodNanos = SwappyGL_getRefreshPeriodNanos;
    glfn->getSwapIntervalNS = SwappyGL_getSwapIntervalNS;
    glfn->getUseAffinity = SwappyGL_getUseAffinity;
    glfn->getFenceTimeoutNS = SwappyGL_getFenceTimeoutNS;
    glfn->setBufferStuffingFixWait = SwappyGL_setBufferStuffingFixWait;
    glfn->onChoreographer = SwappyGL_onChoreographer;
    glfn->injectTracer = SwappyGL_injectTracer;
    glfn->setAutoSwapInterval = SwappyGL_setAutoSwapInterval;
    glfn->setMaxAutoSwapIntervalNS = SwappyGL_setMaxAutoSwapIntervalNS;
    glfn->setAutoPipelineMode = SwappyGL_setAutoPipelineMode;
    glfn->enableStats = SwappyGL_enableStats;
    glfn->recordFrameStart = SwappyGL_recordFrameStart;
    glfn->getStats = SwappyGL_getStats;
    glfn->uninjectTracer = SwappyGL_uninjectTracer;
    
    vkfn->determineDeviceExtensions = SwappyVk_determineDeviceExtensions;
    vkfn->setQueueFamilyIndex = SwappyVk_setQueueFamilyIndex;
    vkfn->initAndGetRefreshCycleDuration = SwappyVk_initAndGetRefreshCycleDuration;
    vkfn->setWindow = SwappyVk_setWindow;
    vkfn->setSwapIntervalNS = SwappyVk_setSwapIntervalNS;
    vkfn->queuePresent = SwappyVk_queuePresent;
    vkfn->destroySwapchain = SwappyVk_destroySwapchain;
    vkfn->destroyDevice = SwappyVk_destroyDevice;
    vkfn->setAutoSwapInterval = SwappyVk_setAutoSwapInterval;
    vkfn->setAutoPipelineMode = SwappyVk_setAutoPipelineMode;
    vkfn->setMaxAutoSwapIntervalNS = SwappyVk_setMaxAutoSwapIntervalNS;
    vkfn->setFenceTimeoutNS = SwappyVk_setFenceTimeoutNS;
    vkfn->getFenceTimeoutNS = SwappyVk_getFenceTimeoutNS;
    vkfn->injectTracer = SwappyVk_injectTracer;
    vkfn->setFunctionProvider = SwappyWrapperSetFunctionProvider;
    vkfn->getSwapIntervalNS = SwappyVk_getSwapIntervalNS;

    __real___android_log_print(ANDROID_LOG_INFO, "SwappyWrapper", "SwappyWrapperInit() succeeded. Build: %s", __TIMESTAMP__);
    
    return true;
}


// Control excessive logging from Swappy.
// Ideally we would use __android_log_set_minimum_priority(), but it's only available in API level 30 and above.
extern "C" int __wrap___android_log_print(int prio, const char* tag, const char* fmt, ...)
{
    // only log fatal errors from Swappy
    if (prio < UNITY_MIN_SWAPPY_LOG_PRIORITY)
        return 0;

    va_list args;
    va_start(args, fmt);
    int ret = __android_log_vprint(prio, tag, fmt, args);
    va_end(args);
    return ret;
}

#endif
