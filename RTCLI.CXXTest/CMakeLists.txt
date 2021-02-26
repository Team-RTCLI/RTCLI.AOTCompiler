cmake_minimum_required(VERSION 3.19.0)

set(CMAKE_C_STANDARD 11)
set(CMAKE_CXX_STANDARD 17)
if(WIN32)
    set(CMAKE_PREFIX_PATH "C:/Program Files (x86)")
endif()

project(RTCLI_CXXTest)

set(GEN_TEST TRUE)

find_package(RTCLIDefault 0.0.1 REQUIRED)
link_libraries(RTCLI::RTCLIDefault)
include_directories(RTCLI.Generated)

FILE(GLOB modules
    RELATIVE ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/*
)

# build all assemblies
FOREACH(module ${modules})
    message(STATUS ${module})
    file(GLOB_RECURSE 
        inc_list
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.h
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.hpp
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.hxx
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.inl
    ) 
    file(GLOB_RECURSE 
        source_list 
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.c
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.cpp
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/*.cxx
    )
    add_library(${module} ${inc_list} ${source_list})
    target_include_directories(${module} 
        PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/include
    )

    if(GEN_TEST)
        FOREACH(test ${source_list})
            get_filename_component(TARGET_NAME ${test} NAME_WLE)
            set(TARGET_NAME ${module}_${TARGET_NAME})
            message(STATUS ${TARGET_NAME})

            add_executable(${TARGET_NAME} ${test})
            target_compile_definitions(${TARGET_NAME} PRIVATE "-DRTCLI_TEST_POINT")
            target_include_directories(${TARGET_NAME} PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/RTCLI.Generated/${module}/include)
        ENDFOREACH()
    endif(GEN_TEST)
ENDFOREACH()

#add_definitions("-DRTCLI_TEST_POINT")
