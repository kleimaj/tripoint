// <auto-generated/>
#pragma warning disable
using Marten.Internal;
using Marten.Internal.Storage;
using Marten.Schema;
using Marten.Schema.Arguments;
using Npgsql;
using System;
using System.Collections.Generic;
using TriPoint.Models;
using Weasel.Core;
using Weasel.Postgresql;

namespace Marten.Generated.DocumentStorage
{
    // START: UpsertContactUsOperation756967825
    public class UpsertContactUsOperation756967825 : Marten.Internal.Operations.StorageOperation<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly TriPoint.Models.ContactUs _document;
        private readonly System.Guid _id;
        private readonly System.Collections.Generic.Dictionary<System.Guid, System.Guid> _versions;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public UpsertContactUsOperation756967825(TriPoint.Models.ContactUs document, System.Guid id, System.Collections.Generic.Dictionary<System.Guid, System.Guid> versions, Marten.Schema.DocumentMapping mapping) : base(document, id, versions, mapping)
        {
            _document = document;
            _id = id;
            _versions = versions;
            _mapping = mapping;
        }


        public const string COMMAND_TEXT = "select public.mt_upsert_contactus(?, ?, ?, ?)";


        public override void Postprocess(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions)
        {
            storeVersion();
        }


        public override System.Threading.Tasks.Task PostprocessAsync(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions, System.Threading.CancellationToken token)
        {
            storeVersion();
            // Nothing
            return System.Threading.Tasks.Task.CompletedTask;
        }


        public override Marten.Internal.Operations.OperationRole Role()
        {
            return Marten.Internal.Operations.OperationRole.Upsert;
        }


        public override string CommandText()
        {
            return COMMAND_TEXT;
        }


        public override NpgsqlTypes.NpgsqlDbType DbType()
        {
            return NpgsqlTypes.NpgsqlDbType.Uuid;
        }


        public override void ConfigureParameters(Npgsql.NpgsqlParameter[] parameters, TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session)
        {
            parameters[0].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
            parameters[0].Value = session.Serializer.ToJson(_document);
            // .Net Class Type
            parameters[1].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            parameters[1].Value = _document.GetType().FullName;
            parameters[2].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
            parameters[2].Value = document.Id;
            setVersionParameter(parameters[3]);
        }

    }

    // END: UpsertContactUsOperation756967825
    
    
    // START: InsertContactUsOperation756967825
    public class InsertContactUsOperation756967825 : Marten.Internal.Operations.StorageOperation<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly TriPoint.Models.ContactUs _document;
        private readonly System.Guid _id;
        private readonly System.Collections.Generic.Dictionary<System.Guid, System.Guid> _versions;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public InsertContactUsOperation756967825(TriPoint.Models.ContactUs document, System.Guid id, System.Collections.Generic.Dictionary<System.Guid, System.Guid> versions, Marten.Schema.DocumentMapping mapping) : base(document, id, versions, mapping)
        {
            _document = document;
            _id = id;
            _versions = versions;
            _mapping = mapping;
        }


        public const string COMMAND_TEXT = "select public.mt_insert_contactus(?, ?, ?, ?)";


        public override void Postprocess(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions)
        {
            storeVersion();
        }


        public override System.Threading.Tasks.Task PostprocessAsync(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions, System.Threading.CancellationToken token)
        {
            storeVersion();
            // Nothing
            return System.Threading.Tasks.Task.CompletedTask;
        }


        public override Marten.Internal.Operations.OperationRole Role()
        {
            return Marten.Internal.Operations.OperationRole.Insert;
        }


        public override string CommandText()
        {
            return COMMAND_TEXT;
        }


        public override NpgsqlTypes.NpgsqlDbType DbType()
        {
            return NpgsqlTypes.NpgsqlDbType.Uuid;
        }


        public override void ConfigureParameters(Npgsql.NpgsqlParameter[] parameters, TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session)
        {
            parameters[0].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
            parameters[0].Value = session.Serializer.ToJson(_document);
            // .Net Class Type
            parameters[1].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            parameters[1].Value = _document.GetType().FullName;
            parameters[2].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
            parameters[2].Value = document.Id;
            setVersionParameter(parameters[3]);
        }

    }

    // END: InsertContactUsOperation756967825
    
    
    // START: UpdateContactUsOperation756967825
    public class UpdateContactUsOperation756967825 : Marten.Internal.Operations.StorageOperation<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly TriPoint.Models.ContactUs _document;
        private readonly System.Guid _id;
        private readonly System.Collections.Generic.Dictionary<System.Guid, System.Guid> _versions;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public UpdateContactUsOperation756967825(TriPoint.Models.ContactUs document, System.Guid id, System.Collections.Generic.Dictionary<System.Guid, System.Guid> versions, Marten.Schema.DocumentMapping mapping) : base(document, id, versions, mapping)
        {
            _document = document;
            _id = id;
            _versions = versions;
            _mapping = mapping;
        }


        public const string COMMAND_TEXT = "select public.mt_update_contactus(?, ?, ?, ?)";


        public override void Postprocess(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions)
        {
            storeVersion();
            postprocessUpdate(reader, exceptions);
        }


        public override async System.Threading.Tasks.Task PostprocessAsync(System.Data.Common.DbDataReader reader, System.Collections.Generic.IList<System.Exception> exceptions, System.Threading.CancellationToken token)
        {
            storeVersion();
            await postprocessUpdateAsync(reader, exceptions, token);
        }


        public override Marten.Internal.Operations.OperationRole Role()
        {
            return Marten.Internal.Operations.OperationRole.Update;
        }


        public override string CommandText()
        {
            return COMMAND_TEXT;
        }


        public override NpgsqlTypes.NpgsqlDbType DbType()
        {
            return NpgsqlTypes.NpgsqlDbType.Uuid;
        }


        public override void ConfigureParameters(Npgsql.NpgsqlParameter[] parameters, TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session)
        {
            parameters[0].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
            parameters[0].Value = session.Serializer.ToJson(_document);
            // .Net Class Type
            parameters[1].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Varchar;
            parameters[1].Value = _document.GetType().FullName;
            parameters[2].NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Uuid;
            parameters[2].Value = document.Id;
            setVersionParameter(parameters[3]);
        }

    }

    // END: UpdateContactUsOperation756967825
    
    
    // START: QueryOnlyContactUsSelector756967825
    public class QueryOnlyContactUsSelector756967825 : Marten.Internal.CodeGeneration.DocumentSelectorWithOnlySerializer, Marten.Linq.Selectors.ISelector<TriPoint.Models.ContactUs>
    {
        private readonly Marten.Internal.IMartenSession _session;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public QueryOnlyContactUsSelector756967825(Marten.Internal.IMartenSession session, Marten.Schema.DocumentMapping mapping) : base(session, mapping)
        {
            _session = session;
            _mapping = mapping;
        }



        public TriPoint.Models.ContactUs Resolve(System.Data.Common.DbDataReader reader)
        {

            TriPoint.Models.ContactUs document;
            document = _serializer.FromJson<TriPoint.Models.ContactUs>(reader, 0);
            return document;
        }


        public async System.Threading.Tasks.Task<TriPoint.Models.ContactUs> ResolveAsync(System.Data.Common.DbDataReader reader, System.Threading.CancellationToken token)
        {

            TriPoint.Models.ContactUs document;
            document = await _serializer.FromJsonAsync<TriPoint.Models.ContactUs>(reader, 0, token).ConfigureAwait(false);
            return document;
        }

    }

    // END: QueryOnlyContactUsSelector756967825
    
    
    // START: LightweightContactUsSelector756967825
    public class LightweightContactUsSelector756967825 : Marten.Internal.CodeGeneration.DocumentSelectorWithVersions<TriPoint.Models.ContactUs, System.Guid>, Marten.Linq.Selectors.ISelector<TriPoint.Models.ContactUs>
    {
        private readonly Marten.Internal.IMartenSession _session;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public LightweightContactUsSelector756967825(Marten.Internal.IMartenSession session, Marten.Schema.DocumentMapping mapping) : base(session, mapping)
        {
            _session = session;
            _mapping = mapping;
        }



        public TriPoint.Models.ContactUs Resolve(System.Data.Common.DbDataReader reader)
        {
            var id = reader.GetFieldValue<System.Guid>(0);

            TriPoint.Models.ContactUs document;
            document = _serializer.FromJson<TriPoint.Models.ContactUs>(reader, 1);
            _session.MarkAsDocumentLoaded(id, document);
            return document;
        }


        public async System.Threading.Tasks.Task<TriPoint.Models.ContactUs> ResolveAsync(System.Data.Common.DbDataReader reader, System.Threading.CancellationToken token)
        {
            var id = await reader.GetFieldValueAsync<System.Guid>(0, token);

            TriPoint.Models.ContactUs document;
            document = await _serializer.FromJsonAsync<TriPoint.Models.ContactUs>(reader, 1, token).ConfigureAwait(false);
            _session.MarkAsDocumentLoaded(id, document);
            return document;
        }

    }

    // END: LightweightContactUsSelector756967825
    
    
    // START: IdentityMapContactUsSelector756967825
    public class IdentityMapContactUsSelector756967825 : Marten.Internal.CodeGeneration.DocumentSelectorWithIdentityMap<TriPoint.Models.ContactUs, System.Guid>, Marten.Linq.Selectors.ISelector<TriPoint.Models.ContactUs>
    {
        private readonly Marten.Internal.IMartenSession _session;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public IdentityMapContactUsSelector756967825(Marten.Internal.IMartenSession session, Marten.Schema.DocumentMapping mapping) : base(session, mapping)
        {
            _session = session;
            _mapping = mapping;
        }



        public TriPoint.Models.ContactUs Resolve(System.Data.Common.DbDataReader reader)
        {
            var id = reader.GetFieldValue<System.Guid>(0);
            if (_identityMap.TryGetValue(id, out var existing)) return existing;

            TriPoint.Models.ContactUs document;
            document = _serializer.FromJson<TriPoint.Models.ContactUs>(reader, 1);
            _session.MarkAsDocumentLoaded(id, document);
            _identityMap[id] = document;
            return document;
        }


        public async System.Threading.Tasks.Task<TriPoint.Models.ContactUs> ResolveAsync(System.Data.Common.DbDataReader reader, System.Threading.CancellationToken token)
        {
            var id = await reader.GetFieldValueAsync<System.Guid>(0, token);
            if (_identityMap.TryGetValue(id, out var existing)) return existing;

            TriPoint.Models.ContactUs document;
            document = await _serializer.FromJsonAsync<TriPoint.Models.ContactUs>(reader, 1, token).ConfigureAwait(false);
            _session.MarkAsDocumentLoaded(id, document);
            _identityMap[id] = document;
            return document;
        }

    }

    // END: IdentityMapContactUsSelector756967825
    
    
    // START: DirtyTrackingContactUsSelector756967825
    public class DirtyTrackingContactUsSelector756967825 : Marten.Internal.CodeGeneration.DocumentSelectorWithDirtyChecking<TriPoint.Models.ContactUs, System.Guid>, Marten.Linq.Selectors.ISelector<TriPoint.Models.ContactUs>
    {
        private readonly Marten.Internal.IMartenSession _session;
        private readonly Marten.Schema.DocumentMapping _mapping;

        public DirtyTrackingContactUsSelector756967825(Marten.Internal.IMartenSession session, Marten.Schema.DocumentMapping mapping) : base(session, mapping)
        {
            _session = session;
            _mapping = mapping;
        }



        public TriPoint.Models.ContactUs Resolve(System.Data.Common.DbDataReader reader)
        {
            var id = reader.GetFieldValue<System.Guid>(0);
            if (_identityMap.TryGetValue(id, out var existing)) return existing;

            TriPoint.Models.ContactUs document;
            document = _serializer.FromJson<TriPoint.Models.ContactUs>(reader, 1);
            _session.MarkAsDocumentLoaded(id, document);
            _identityMap[id] = document;
            StoreTracker(_session, document);
            return document;
        }


        public async System.Threading.Tasks.Task<TriPoint.Models.ContactUs> ResolveAsync(System.Data.Common.DbDataReader reader, System.Threading.CancellationToken token)
        {
            var id = await reader.GetFieldValueAsync<System.Guid>(0, token);
            if (_identityMap.TryGetValue(id, out var existing)) return existing;

            TriPoint.Models.ContactUs document;
            document = await _serializer.FromJsonAsync<TriPoint.Models.ContactUs>(reader, 1, token).ConfigureAwait(false);
            _session.MarkAsDocumentLoaded(id, document);
            _identityMap[id] = document;
            StoreTracker(_session, document);
            return document;
        }

    }

    // END: DirtyTrackingContactUsSelector756967825
    
    
    // START: QueryOnlyContactUsDocumentStorage756967825
    public class QueryOnlyContactUsDocumentStorage756967825 : Marten.Internal.Storage.QueryOnlyDocumentStorage<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly Marten.Schema.DocumentMapping _document;

        public QueryOnlyContactUsDocumentStorage756967825(Marten.Schema.DocumentMapping document) : base(document)
        {
            _document = document;
        }



        public override System.Guid AssignIdentity(TriPoint.Models.ContactUs document, string tenantId, Marten.Storage.IMartenDatabase database)
        {
            if (document.Id == Guid.Empty) _setter(document, Marten.Schema.Identity.CombGuidIdGeneration.NewGuid());
            return document.Id;
        }


        public override Marten.Internal.Operations.IStorageOperation Update(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpdateContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Insert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.InsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Upsert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Overwrite(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {
            throw new System.NotSupportedException();
        }


        public override System.Guid Identity(TriPoint.Models.ContactUs document)
        {
            return document.Id;
        }


        public override Marten.Linq.Selectors.ISelector BuildSelector(Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.DocumentStorage.QueryOnlyContactUsSelector756967825(session, _document);
        }


        public override Npgsql.NpgsqlCommand BuildLoadCommand(System.Guid id, string tenant)
        {
            return new NpgsqlCommand(_loaderSql).With("id", id);
        }


        public override Npgsql.NpgsqlCommand BuildLoadManyCommand(System.Guid[] ids, string tenant)
        {
            return new NpgsqlCommand(_loadArraySql).With("ids", ids);
        }

    }

    // END: QueryOnlyContactUsDocumentStorage756967825
    
    
    // START: LightweightContactUsDocumentStorage756967825
    public class LightweightContactUsDocumentStorage756967825 : Marten.Internal.Storage.LightweightDocumentStorage<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly Marten.Schema.DocumentMapping _document;

        public LightweightContactUsDocumentStorage756967825(Marten.Schema.DocumentMapping document) : base(document)
        {
            _document = document;
        }



        public override System.Guid AssignIdentity(TriPoint.Models.ContactUs document, string tenantId, Marten.Storage.IMartenDatabase database)
        {
            if (document.Id == Guid.Empty) _setter(document, Marten.Schema.Identity.CombGuidIdGeneration.NewGuid());
            return document.Id;
        }


        public override Marten.Internal.Operations.IStorageOperation Update(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpdateContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Insert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.InsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Upsert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Overwrite(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {
            throw new System.NotSupportedException();
        }


        public override System.Guid Identity(TriPoint.Models.ContactUs document)
        {
            return document.Id;
        }


        public override Marten.Linq.Selectors.ISelector BuildSelector(Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.DocumentStorage.LightweightContactUsSelector756967825(session, _document);
        }


        public override Npgsql.NpgsqlCommand BuildLoadCommand(System.Guid id, string tenant)
        {
            return new NpgsqlCommand(_loaderSql).With("id", id);
        }


        public override Npgsql.NpgsqlCommand BuildLoadManyCommand(System.Guid[] ids, string tenant)
        {
            return new NpgsqlCommand(_loadArraySql).With("ids", ids);
        }

    }

    // END: LightweightContactUsDocumentStorage756967825
    
    
    // START: IdentityMapContactUsDocumentStorage756967825
    public class IdentityMapContactUsDocumentStorage756967825 : Marten.Internal.Storage.IdentityMapDocumentStorage<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly Marten.Schema.DocumentMapping _document;

        public IdentityMapContactUsDocumentStorage756967825(Marten.Schema.DocumentMapping document) : base(document)
        {
            _document = document;
        }



        public override System.Guid AssignIdentity(TriPoint.Models.ContactUs document, string tenantId, Marten.Storage.IMartenDatabase database)
        {
            if (document.Id == Guid.Empty) _setter(document, Marten.Schema.Identity.CombGuidIdGeneration.NewGuid());
            return document.Id;
        }


        public override Marten.Internal.Operations.IStorageOperation Update(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpdateContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Insert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.InsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Upsert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Overwrite(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {
            throw new System.NotSupportedException();
        }


        public override System.Guid Identity(TriPoint.Models.ContactUs document)
        {
            return document.Id;
        }


        public override Marten.Linq.Selectors.ISelector BuildSelector(Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.DocumentStorage.IdentityMapContactUsSelector756967825(session, _document);
        }


        public override Npgsql.NpgsqlCommand BuildLoadCommand(System.Guid id, string tenant)
        {
            return new NpgsqlCommand(_loaderSql).With("id", id);
        }


        public override Npgsql.NpgsqlCommand BuildLoadManyCommand(System.Guid[] ids, string tenant)
        {
            return new NpgsqlCommand(_loadArraySql).With("ids", ids);
        }

    }

    // END: IdentityMapContactUsDocumentStorage756967825
    
    
    // START: DirtyTrackingContactUsDocumentStorage756967825
    public class DirtyTrackingContactUsDocumentStorage756967825 : Marten.Internal.Storage.DirtyCheckedDocumentStorage<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly Marten.Schema.DocumentMapping _document;

        public DirtyTrackingContactUsDocumentStorage756967825(Marten.Schema.DocumentMapping document) : base(document)
        {
            _document = document;
        }



        public override System.Guid AssignIdentity(TriPoint.Models.ContactUs document, string tenantId, Marten.Storage.IMartenDatabase database)
        {
            if (document.Id == Guid.Empty) _setter(document, Marten.Schema.Identity.CombGuidIdGeneration.NewGuid());
            return document.Id;
        }


        public override Marten.Internal.Operations.IStorageOperation Update(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpdateContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Insert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.InsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Upsert(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {

            return new Marten.Generated.DocumentStorage.UpsertContactUsOperation756967825
            (
                document, Identity(document),
                session.Versions.ForType<TriPoint.Models.ContactUs, System.Guid>(),
                _document
                
            );
        }


        public override Marten.Internal.Operations.IStorageOperation Overwrite(TriPoint.Models.ContactUs document, Marten.Internal.IMartenSession session, string tenant)
        {
            throw new System.NotSupportedException();
        }


        public override System.Guid Identity(TriPoint.Models.ContactUs document)
        {
            return document.Id;
        }


        public override Marten.Linq.Selectors.ISelector BuildSelector(Marten.Internal.IMartenSession session)
        {
            return new Marten.Generated.DocumentStorage.DirtyTrackingContactUsSelector756967825(session, _document);
        }


        public override Npgsql.NpgsqlCommand BuildLoadCommand(System.Guid id, string tenant)
        {
            return new NpgsqlCommand(_loaderSql).With("id", id);
        }


        public override Npgsql.NpgsqlCommand BuildLoadManyCommand(System.Guid[] ids, string tenant)
        {
            return new NpgsqlCommand(_loadArraySql).With("ids", ids);
        }

    }

    // END: DirtyTrackingContactUsDocumentStorage756967825
    
    
    // START: ContactUsBulkLoader756967825
    public class ContactUsBulkLoader756967825 : Marten.Internal.CodeGeneration.BulkLoader<TriPoint.Models.ContactUs, System.Guid>
    {
        private readonly Marten.Internal.Storage.IDocumentStorage<TriPoint.Models.ContactUs, System.Guid> _storage;

        public ContactUsBulkLoader756967825(Marten.Internal.Storage.IDocumentStorage<TriPoint.Models.ContactUs, System.Guid> storage) : base(storage)
        {
            _storage = storage;
        }


        public const string MAIN_LOADER_SQL = "COPY public.mt_doc_contactus(\"mt_dotnet_type\", \"id\", \"mt_version\", \"data\") FROM STDIN BINARY";

        public const string TEMP_LOADER_SQL = "COPY mt_doc_contactus_temp(\"mt_dotnet_type\", \"id\", \"mt_version\", \"data\") FROM STDIN BINARY";

        public const string COPY_NEW_DOCUMENTS_SQL = "insert into public.mt_doc_contactus (\"id\", \"data\", \"mt_version\", \"mt_dotnet_type\", mt_last_modified) (select mt_doc_contactus_temp.\"id\", mt_doc_contactus_temp.\"data\", mt_doc_contactus_temp.\"mt_version\", mt_doc_contactus_temp.\"mt_dotnet_type\", transaction_timestamp() from mt_doc_contactus_temp left join public.mt_doc_contactus on mt_doc_contactus_temp.id = public.mt_doc_contactus.id where public.mt_doc_contactus.id is null)";

        public const string OVERWRITE_SQL = "update public.mt_doc_contactus target SET data = source.data, mt_version = source.mt_version, mt_dotnet_type = source.mt_dotnet_type, mt_last_modified = transaction_timestamp() FROM mt_doc_contactus_temp source WHERE source.id = target.id";

        public const string CREATE_TEMP_TABLE_FOR_COPYING_SQL = "create temporary table mt_doc_contactus_temp as select * from public.mt_doc_contactus limit 0";


        public override string CreateTempTableForCopying()
        {
            return CREATE_TEMP_TABLE_FOR_COPYING_SQL;
        }


        public override string CopyNewDocumentsFromTempTable()
        {
            return COPY_NEW_DOCUMENTS_SQL;
        }


        public override string OverwriteDuplicatesFromTempTable()
        {
            return OVERWRITE_SQL;
        }


        public override void LoadRow(Npgsql.NpgsqlBinaryImporter writer, TriPoint.Models.ContactUs document, Marten.Storage.Tenant tenant, Marten.ISerializer serializer)
        {
            writer.Write(document.GetType().FullName, NpgsqlTypes.NpgsqlDbType.Varchar);
            writer.Write(document.Id, NpgsqlTypes.NpgsqlDbType.Uuid);
            writer.Write(Marten.Schema.Identity.CombGuidIdGeneration.NewGuid(), NpgsqlTypes.NpgsqlDbType.Uuid);
            writer.Write(serializer.ToJson(document), NpgsqlTypes.NpgsqlDbType.Jsonb);
        }


        public override async System.Threading.Tasks.Task LoadRowAsync(Npgsql.NpgsqlBinaryImporter writer, TriPoint.Models.ContactUs document, Marten.Storage.Tenant tenant, Marten.ISerializer serializer, System.Threading.CancellationToken cancellation)
        {
            await writer.WriteAsync(document.GetType().FullName, NpgsqlTypes.NpgsqlDbType.Varchar, cancellation);
            await writer.WriteAsync(document.Id, NpgsqlTypes.NpgsqlDbType.Uuid, cancellation);
            await writer.WriteAsync(Marten.Schema.Identity.CombGuidIdGeneration.NewGuid(), NpgsqlTypes.NpgsqlDbType.Uuid, cancellation);
            await writer.WriteAsync(serializer.ToJson(document), NpgsqlTypes.NpgsqlDbType.Jsonb, cancellation);
        }


        public override string MainLoaderSql()
        {
            return MAIN_LOADER_SQL;
        }


        public override string TempLoaderSql()
        {
            return TEMP_LOADER_SQL;
        }

    }

    // END: ContactUsBulkLoader756967825
    
    
    // START: ContactUsProvider756967825
    public class ContactUsProvider756967825 : Marten.Internal.Storage.DocumentProvider<TriPoint.Models.ContactUs>
    {
        private readonly Marten.Schema.DocumentMapping _mapping;

        public ContactUsProvider756967825(Marten.Schema.DocumentMapping mapping) : base(new ContactUsBulkLoader756967825(new QueryOnlyContactUsDocumentStorage756967825(mapping)), new QueryOnlyContactUsDocumentStorage756967825(mapping), new LightweightContactUsDocumentStorage756967825(mapping), new IdentityMapContactUsDocumentStorage756967825(mapping), new DirtyTrackingContactUsDocumentStorage756967825(mapping))
        {
            _mapping = mapping;
        }


    }

    // END: ContactUsProvider756967825
    
    
}

