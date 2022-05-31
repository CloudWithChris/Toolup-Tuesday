param prefix string = 'tut'

param location string = resourceGroup().location

param containerAppImage string = 'ghcr.io/cloudwithchris/toolup-tuesday/spacebar/player-decisions:183b45616542ae7a893a03394904cc7eed91cffb'

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

resource workspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    workspaceCapping: {}
  }
}

resource sa 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {}
}

//Container App Environment
resource environment 'Microsoft.App/managedEnvironments@2022-01-01-preview'= {
  name: containerAppEnvironmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: workspace.properties.customerId
        sharedKey: listKeys(workspace.id, workspace.apiVersion).primarySharedKey
      }
    }
  }
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

