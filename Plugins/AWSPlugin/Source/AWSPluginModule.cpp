// Copyright 2025 (c) Tangha Technologies, LLC. All Rights Reserved.

#include "AWSPluginModule.h"

#define LOCTEXT_NAMESPACE "FAWSPluginModule"


DEFINE_LOG_CATEGORY(LogAWSPlugin)

void FAWSPluginModule::StartupModule()
{
}

void FAWSPluginModule::ShutdownModule()
{
}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FAWSPluginModule, AWSPlugin)
