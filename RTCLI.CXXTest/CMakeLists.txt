cmake_minimum_required(VERSION 3.19.0)

set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)
if(WIN32)
    set(CMAKE_PREFIX_PATH "C:/Program Files (x86)")
endif()

find_package(RTCLIDefault 0.0.1 REQUIRED)
link_libraries(RTCLI::RTCLIDefault)
include_directories(RTCLI.Generated/include)

FILE(GLOB modules
    RELATIVE ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/src ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/src/*
)

add_definitions("-DRTCLI_TEST_POINT")

FOREACH(module ${modules})
    message(STATUS ${module})
    project(${module})
    FILE(GLOB srcs
        RELATIVE ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/src/${module}
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/src/${module}/*
    )
    FOREACH(src ${srcs})
        get_filename_component(TARGET_NAME ${src} NAME_WLE)
        message(STATUS "TestCase: ${TARGET_NAME}")
        add_executable(${TARGET_NAME}  ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/src/${module}/${src})
    ENDFOREACH()
ENDFOREACH()