// Copyright 2025 (c) Tangha Technologies, LLC. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "Modules/ModuleManager.h"
#pragma warning(disable:4800)

DECLARE_LOG_CATEGORY_EXTERN(LogAWSPlugin, All, All);

class AWSPLUGIN_API FAWSPluginModule : public IModuleInterface
{
public:

	/** IModuleInterface implementation */
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
};
