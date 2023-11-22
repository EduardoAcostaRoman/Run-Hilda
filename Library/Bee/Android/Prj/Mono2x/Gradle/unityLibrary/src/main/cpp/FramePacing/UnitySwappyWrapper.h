#pragma once

// This file is shared between the engine and libswappywrapper.so

#include <stdint.h>
#include <jni.h>
#include <android/native_window.h>
#include <EGL/egl.h>
#include <EGL/eglext.h>

#ifndef UNITY_VULKAN_HEADER
#define UNITY_VULKAN_HEADER <vulkan/vulkan.h>
#endif
#include UNITY_VULKAN_HEADER

#define UNITY_SWAPPYWRAPPER_EXPORT extern "C" __attribute__((visibility("default")))

struct SwappyThreadFunctions;
struct SwappyTracer;
struct SwappyStats;
struct SwappyVkFunctionProvider;

namespace Unity
{
    typedef struct SwappyVkFunctionProvider {
        bool (*init)();
        void* (*getProcAddr)(const char* name);
        void (*close)();
    } SwappyVkFunctionProvider;

    struct SwappyCommonFunctions
    {
        uint32_t (*version)();
        void (*setThreadFunctions)(const SwappyThreadFunctions* thread_functions);
        const char* (*versionString)();
    };

    struct SwappyGLFunctions
    {
        bool (*init)(JNIEnv* env, jobject jactivity);
        bool (*isEnabled)();
        void (*destroy)();
        bool (*setWindow)(ANativeWindow* window);
        bool (*swap)(EGLDisplay display, EGLSurface surface);
        void (*setUseAffinity)(bool tf);
        void (*setSwapIntervalNS)(uint64_t swap_ns);
        void (*setFenceTimeoutNS)(uint64_t fence_timeout_ns);
        uint64_t (*getRefreshPeriodNanos)();
        uint64_t (*getSwapIntervalNS)();
        bool (*getUseAffinity)();
        uint64_t (*getFenceTimeoutNS)();
        void (*setBufferStuffingFixWait)(int32_t n_frames);
        void (*onChoreographer)(int64_t frameTimeNanos);
        void (*injectTracer)(const SwappyTracer* tracer);
        void (*setAutoSwapInterval)(bool enabled);
        void (*setMaxAutoSwapIntervalNS)(uint64_t max_swap_ns);
        void (*setAutoPipelineMode)(bool enabled);
        void (*enableStats)(bool enabled);
        void (*recordFrameStart)(EGLDisplay display, EGLSurface surface);
        void (*getStats)(SwappyStats* swappyStats);
        void (*uninjectTracer)(const SwappyTracer* t);
    };

    struct SwappyVKFunctions
    {
        void (*determineDeviceExtensions)(VkPhysicalDevice physicalDevice, uint32_t availableExtensionCount, VkExtensionProperties* pAvailableExtensions, uint32_t* pRequiredExtensionCount, char** pRequiredExtensions);
        void (*setQueueFamilyIndex)(VkDevice device, VkQueue queue, uint32_t queueFamilyIndex);
        bool (*initAndGetRefreshCycleDuration)(JNIEnv* env, jobject jactivity, VkPhysicalDevice physicalDevice, VkDevice device, VkSwapchainKHR swapchain, uint64_t* pRefreshDuration);
        void (*setWindow)(VkDevice device, VkSwapchainKHR swapchain, ANativeWindow* window);
        void (*setSwapIntervalNS)(VkDevice device, VkSwapchainKHR swapchain, uint64_t swap_ns);
        VkResult (*queuePresent)(VkQueue queue, const VkPresentInfoKHR* pPresentInfo);
        void (*destroySwapchain)(VkDevice device, VkSwapchainKHR swapchain);
        void (*destroyDevice)(VkDevice device);
        void (*setAutoSwapInterval)(bool enabled);
        void (*setAutoPipelineMode)(bool enabled);
        void (*setMaxAutoSwapIntervalNS)(uint64_t max_swap_ns);
        void (*setFenceTimeoutNS)(uint64_t fence_timeout_ns);
        uint64_t (*getFenceTimeoutNS)();
        void (*injectTracer)(const SwappyTracer* tracer);
        void (*setFunctionProvider)(const SwappyVkFunctionProvider* pSwappyVkFunctionProvider);
        uint64_t(*getSwapIntervalNS)(VkSwapchainKHR swapchain);
    };
}

