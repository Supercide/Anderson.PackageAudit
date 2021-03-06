﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anderson.PackageAudit.Audit.Pipes;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Domain;
using Anderson.PackageAudit.Errors;
using Anderson.PackageAudit.Keys.Errors;
using Anderson.PackageAudit.SharedPipes.Authorization.Errors;
using Anderson.Pipelines.Responses;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Anderson.PackageAudit.Keys.Pipelines
{
    public class KeyCreationPipe : Anderson.Pipelines.Definitions.PipelineDefinition<KeyRequest, Response<Key, Error>>
    {
        private readonly IMongoCollection<Tenant> _tenantCollection;

        public KeyCreationPipe(IMongoCollection<Tenant> tenantCollection)
        {
            _tenantCollection = tenantCollection;
        }

        public override Response<Key, Error> Handle(KeyRequest request)
        {
            var account = Thread.CurrentPrincipal.ToAccount();

            var tenant = _tenantCollection.FindSync(x => x.Accounts.Contains(account))
                                          .FirstOrDefault();

            if (tenant == null)
            {
                return TenantError.UnknownTenant;
            }

            var key = tenant.GenerateKey(request.Name);

            _tenantCollection.ReplaceOne(x => x.Id == tenant.Id, tenant);

            return key;
        }
    }
}