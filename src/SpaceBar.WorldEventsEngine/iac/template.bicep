param prefix string = 'tutspace'

param location string = resourceGroup().location

param containerAppImage string = 'ghcr.io/cloudwithchris/toolup-tuesday/spacebar/world-events-engine:0128fa1d5315c9e50d0ba672f8f26cde4e20af94'

@secure()
param containerRegistryPassword string

var containerAppEnvironmentName = '${prefix}-environment'
var containerAppName = '${prefix}-worldeventsengine'
var containerRegistryLoginServer = 'ghcr.io'
var containerRegistryUserName = 'chrisreddington'

var storageAccountName = '${prefix}storage'
var storageAccountKeyRef = 'storage-account-key'
var storageQueueInputName = 'worldeventsqueue'
var containerRegistryPasswordRef = 'container-registry-password'

resource sa 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
  name: storageAccountName
}

//Container App Environment
resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview' existing = {
  name: containerAppEnvironmentName
}

resource worldEventsInputBinding 'Microsoft.App/managedEnvironments/daprComponents@2022-03-01' = {
  name: 'world-events'
  parent: environment
  properties: {
    componentType: 'bindings.azure.storagequeues'
    version: 'v1'
    metadata: [
      {
        name: 'storageAccount'
        value: storageAccountName
      }
      {
        name: 'storageAccessKey'
        secretRef: storageAccountKeyRef
      }
      {
        name: 'queue'
        value: storageQueueInputName
      }
      {
        name: 'decodeBase64'
        value: 'false'
      }
    ]
    scopes: [
      'worldeventsengine'
    ]
    secrets: [
      {
        name: storageAccountKeyRef
        value: listKeys(sa.id, sa.apiVersion).keys[0].value
      }
    ]
  }
}

resource worldeventsstoragestate 'Microsoft.App/managedEnvironments/daprComponents@2022-03-01' = {
  name: 'worldevents'
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
        value: 'worldevents'
      } 
    ]
    scopes: [
      'worldeventsengine'
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
        targetPort: 5016
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
        appId: 'worldeventsengine'
        appPort: 5016
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
          name: 'worldeventsengine'
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 1
      }
    }
    
  }
}

