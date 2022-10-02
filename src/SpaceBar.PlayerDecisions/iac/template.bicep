param prefix string = 'tutspace'

param location string = resourceGroup().location

param containerAppImage string = 'ghcr.io/cloudwithchris/toolup-tuesday/spacebar/player-decisions:c133a5b9ab291949cbfb3c70655c5dd523a36f5d'

@secure()
param containerRegistryPassword string

var containerAppEnvironmentName = '${prefix}-environment'
var containerAppName = '${prefix}-playerdecisions'
var containerRegistryLoginServer = 'ghcr.io'
var containerRegistryUserName = 'chrisreddington'

var logAnalyticsWorkspaceName= '${prefix}-logs'
var storageAccountName = '${prefix}storage'
var storageAccountKeyRef = 'storage-account-key'
var containerRegistryPasswordRef = 'container-registry-password'

resource sa 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
  name: storageAccountName
}

//Container App Environment
resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview' existing = {
  name: containerAppEnvironmentName
}

resource playerdecisionstoragestate 'Microsoft.App/managedEnvironments/daprComponents@2022-03-01' = {
  name: 'playerdecisions'
  parent: environment
  properties: {
    componentType: 'state.azure.tablestorage'
    version: 'v1'
    metadata: [
      {
        name: 'accountName'
        value: storageAccountName
      }
      {
        name: 'accountKey'
        secretRef: storageAccountKeyRef
      }
      {
        name: 'tableName'
        value: 'playerdecisions'
      } 
    ]
    scopes: [
      'playerdecisions'
    ]
    secrets: [
      {
        name: storageAccountKeyRef
        value: listKeys(sa.id, sa.apiVersion).keys[0].value
      }
    ]
  }

}

resource containerApp 'Microsoft.App/containerApps@2022-03-01' = {
  name: containerAppName
  location: location
  properties: {
    managedEnvironmentId: environment.id
    configuration: {
      activeRevisionsMode: 'single'
      ingress: {
        external: true
        targetPort: 8080
      }
      registries: [
        {
          server: containerRegistryLoginServer
          username: containerRegistryUserName
          passwordSecretRef: containerRegistryPasswordRef
        }
      ]
      dapr: {
        enabled: true
        appId: 'playerdecisions'
        appPort: 8080
      }
      secrets : [
        {
          name: containerRegistryPasswordRef
          value: containerRegistryPassword
        }
      ]
    }
    template: {
      containers: [
        {
          image: containerAppImage
          name: 'playerdecisions'
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
    
  }
}

