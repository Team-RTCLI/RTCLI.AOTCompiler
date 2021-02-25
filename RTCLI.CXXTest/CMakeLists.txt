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

# build all assemblies
FOREACH(module ${modules})
    message(STATUS ${module})
    project(${module})
    file(GLOB_RECURSE 
        source_list 
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.h
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.hpp
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.hxx

        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.c
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.cpp
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.cxx
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.inl
    )
    add_library(${module} ${incs} ${source_list})
ENDFOREACH()

#add_definitions("-DRTCLI_TEST_POINT")