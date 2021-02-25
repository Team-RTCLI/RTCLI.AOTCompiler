cmake_minimum_required(VERSION 3.19.0)

set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)
if(WIN32)
    set(CMAKE_PREFIX_PATH "C:/Program Files (x86)")
endif()

find_package(RTCLIDefault 0.0.1 REQUIRED)
link_libraries(RTCLI::RTCLIDefault)
include_directories(RTCLI.Generated)

FILE(GLOB modules
    RELATIVE ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/*
)

add_definitions("-DRTCLI_TEST_POINT")

FOREACH(module ${modules})
    message(STATUS ${module})
    project(${module})
    file(GLOB_RECURSE 
        source_list 
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.cpp
    )
    foreach(test ${source_list})
        get_filename_component(TARGET_NAME ${test} NAME_WLE)
        add_executable(${TARGET_NAME} ${test})
    endforeach()
ENDFOREACH()