DECLARE @ApplicationId INT = 1
DECLARE @ClaimSetId_SIS INT
DECLARE @ClaimSetId_RTI INT
DECLARE @ResourceClaimId_AssessmentMetadata INT
DECLARE @ActionId_Read INT
DECLARE @AuthorizationStrategyId INT

-- Insert new Claim Set named 'RTI Vendor'
IF NOT EXISTS(SELECT 'x' FROM [dbo].[ClaimSets] WHERE ClaimSetName = 'RTI Vendor')
BEGIN
	INSERT [dbo].[ClaimSets] ([ClaimSetName], [Application_ApplicationId])
	VALUES ('RTI Vendor', @ApplicationId)
END

SELECT @ClaimSetId_SIS = [ClaimSetId] FROM [dbo].[ClaimSets] WHERE ClaimSetName = 'SIS Vendor'
SELECT @ClaimSetId_RTI = [ClaimSetId] FROM [dbo].[ClaimSets] WHERE ClaimSetName = 'RTI Vendor'
SELECT @ResourceClaimId_AssessmentMetadata = [ResourceClaimId] FROM [dbo].[ResourceClaims] WHERE ResourceName = 'AssessmentMetadata' AND [ParentResourceClaimId] IS NULL
SELECT @ActionId_Read = [ActionId] FROM [dbo].[Actions] WHERE ActionName = 'Read'
SELECT @AuthorizationStrategyId = [AuthorizationStrategyId] FROM [dbo].[AuthorizationStrategies] WHERE [AuthorizationStrategyName] = 'NoFurtherAuthorizationRequired'

-- Copy all resources from Claim Set 'SIS Vendor'
IF NOT EXISTS(SELECT 'x' FROM [dbo].[ClaimSetResourceClaims] WHERE [ClaimSet_ClaimSetId] = @ClaimSetId_RTI)
BEGIN
	INSERT [dbo].[ClaimSetResourceClaims] ([Action_ActionId], [ClaimSet_ClaimSetId], [ResourceClaim_ResourceClaimId])
	SELECT CSRC1.[Action_ActionId], @ClaimSetId_RTI, CSRC1.[ResourceClaim_ResourceClaimId]
	FROM [dbo].[ClaimSetResourceClaims] CSRC1
	WHERE CSRC1.[ClaimSet_ClaimSetId] = @ClaimSetId_SIS
END

UPDATE [dbo].[ClaimSetResourceClaims]
SET
	[AuthorizationStrategyOverride_AuthorizationStrategyId] = @AuthorizationStrategyId
WHERE 
	[ClaimSet_ClaimSetId] = @ClaimSetId_RTI AND
	[Action_ActionId] = @ActionId_Read AND
	[ResourceClaim_ResourceClaimId] = @ResourceClaimId_AssessmentMetadata


SELECT [ClaimSets].[ClaimSetName], [ResourceClaims].[ResourceClaimId], [ResourceClaims].[ResourceName], [ResourceClaims].[ClaimName], [ChildResourceClaims].[ResourceClaimId], [ChildResourceClaims].[ResourceName], [ChildResourceClaims].[ClaimName], [Actions].[ActionName], [AuthorizationStrategies].[DisplayName], [AuthorizationStrategiesOverride].[DisplayName]
FROM [dbo].[ClaimSetResourceClaims] 
LEFT JOIN [dbo].[ClaimSets] ON [ClaimSetResourceClaims].[ClaimSet_ClaimSetId] = [ClaimSets].[ClaimSetId]
LEFT JOIN [dbo].[ResourceClaims] ON [ClaimSetResourceClaims].[ResourceClaim_ResourceClaimId] = [ResourceClaims].[ResourceClaimId]
LEFT JOIN [dbo].[ResourceClaims] AS [ChildResourceClaims] ON [ResourceClaims].[ResourceClaimId] = [ChildResourceClaims].[ParentResourceClaimId]
LEFT JOIN [dbo].[Actions] ON [ClaimSetResourceClaims].[Action_ActionId] = [Actions].[ActionId]
LEFT JOIN [dbo].[ResourceClaimAuthorizationMetadatas] ON [ClaimSetResourceClaims].[ResourceClaim_ResourceClaimId] = [ResourceClaimAuthorizationMetadatas].[ResourceClaim_ResourceClaimId] AND
														  [ClaimSetResourceClaims].[Action_ActionId] = [ResourceClaimAuthorizationMetadatas].[Action_ActionId] 
LEFT JOIN [dbo].[AuthorizationStrategies] ON [ResourceClaimAuthorizationMetadatas].[AuthorizationStrategy_AuthorizationStrategyId] = [AuthorizationStrategies].[AuthorizationStrategyId]
LEFT JOIN [dbo].[AuthorizationStrategies] as [AuthorizationStrategiesOverride] ON [ClaimSetResourceClaims].[AuthorizationStrategyOverride_AuthorizationStrategyId] = [AuthorizationStrategiesOverride].[AuthorizationStrategyId]
WHERE 
	[ClaimSetName] = 'RTI Vendor' AND 
	[ResourceClaims].[ResourceClaimId] = @ResourceClaimId_AssessmentMetadata
ORDER BY [ResourceClaims].[ResourceName], [ChildResourceClaims].[ResourceName], ActionName
