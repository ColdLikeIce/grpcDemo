using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging.Debug;

namespace Hy.FantasyGrpcEFCore.EFCore
{
    public static class EfCoreAutoMapper
    {
        #region Members

        private static ILogger? _logger;

        #endregion Members

        #region Public static methods

        public static void AutoMap<T>(this ModelBuilder modelBuilder) where T : class
        {
            AutoMap(modelBuilder, typeof(T));
        }

        /// <summary>
        /// Basic method that transform all attributes and
        /// </summary>
        /// <param name="modelBuilder">Builder of EF Core model.</param>
        /// <param name="typeToMap">Type to treat.</param>
        /// <param name="useSchemas">Flag that indicates if current provider can handle schemas.</param>
        /// <param name="loggerFactory">Logger factory</param>
        public static void AutoMap(this ModelBuilder modelBuilder, Type typeToMap, bool useSchemas = true, ILoggerFactory? loggerFactory = null)
        {
            _logger ??= (loggerFactory ?? new LoggerFactory(new[] { new DebugLoggerProvider() })).CreateLogger("EFCoreAutoMapper");

            Log($"Beginning treating mapping for type {typeToMap}", true);

            modelBuilder.Entity(typeToMap, b =>
            {
                ApplyBaseRulesOnBuilder(b, typeToMap);
                AutoMapInternal(b, typeToMap, useSchemas);
                Log($"Ending treating mapping for type {typeToMap}", true);
            });
        }

        #endregion Public static methods

        #region Private static methods

        private static void Log(string message, bool debug = false)
        {
            if (_logger == null)
            {
                return;
            }
        }

        private static void ApplyBaseRulesOnBuilder(EntityTypeBuilder entityBuilder, Type entityType)
        {
            // if (entityType.IsSubclassOf(typeof(BasePersistableEntity)))
            // {
            //     SetBasePropertiesConstraints(entityBuilder);
            // }
            //foreach (var prop in entityType.GetAllProperties().Where(p => p.IsDefined(typeof(NotMappedAttribute))))
            //{
            //    Log($"Property '{prop.Name}' of type '{entityType.Name}' ignored.");
            //    entityBuilder.Ignore(prop.Name);
            //}
        }

        public static void AutoMapInternal(EntityTypeBuilder builder, Type entityType, bool useSQLServer = true)
        {
            var tableAttr = entityType.GetCustomAttribute<TableAttribute>();
            var tableName = !string.IsNullOrWhiteSpace(tableAttr?.Name) ? tableAttr!.Name : entityType.Name;
            Log($"Beginning of treatment for type '{tableName}'");

            if (useSQLServer && !string.IsNullOrWhiteSpace(tableAttr?.Schema))
            {
                builder.ToTable(tableName, tableAttr!.Schema);
            }
            else
            {
                builder.ToTable(tableName);
            }

            CreateColumns(builder, entityType);
            CreatePrimaryKey(builder, entityType);
            // CreateComplexIndexes(builder, entityType);
            // CreateRelations(builder, entityType);
        }

        // private static void SetBasePropertiesConstraints(EntityTypeBuilder entityBuilder)
        // {
        //     entityBuilder.Property(nameof(BasePersistableEntity.EditDate)).IsRequired(true);
        //     entityBuilder.Property(nameof(BasePersistableEntity.Deleted)).IsRequired(true).HasDefaultValue(false);
        //     entityBuilder.Property(nameof(BasePersistableEntity.DeletionDate)).IsRequired(false);
        // }

        private static void CreateColumns(EntityTypeBuilder builder, Type entityType)
        {
            //var properties = entityType.GetAllProperties();

            //foreach (var colProp in properties
            //    .Where(p => p.IsDefined(typeof(ColumnAttribute))))
            //{
            //    var columnAttr = colProp.GetCustomAttribute<ColumnAttribute>();
            //    var columnName = !string.IsNullOrWhiteSpace(columnAttr?.Name) ? columnAttr.Name : colProp.Name;
            //    var propBuilder = builder.Property(colProp.PropertyType, colProp.Name)
            //                             .HasColumnName(columnName);
            //    Log($"Creation of column '{columnName}' for property '{colProp.Name}'.");
            //    ApplyColumnConstrait(builder, colProp, columnName, propBuilder);
            //}

            //foreach (var fkColumn in properties
            //    .Where(p => p.IsDefined(typeof(ForeignKeyAttribute))))
            //{
            //    var kosData = fkColumn.GetCustomAttribute<ForeignKeyAttribute>();
            //    var entityProperty = properties.FirstOrDefault(p => kosData.Name == p.Name);

            //    var distantProperties = entityProperty.PropertyType.GetAllProperties();
            //    var distantTableAttr = entityProperty.PropertyType.GetCustomAttribute<TableAttribute>();
            //    var distantTableName = !string.IsNullOrWhiteSpace(distantTableAttr?.Name) ? distantTableAttr!.Name : entityProperty.PropertyType.Name;

            //    var propBuilder = builder.Property(fkColumn.PropertyType, fkColumn.Name);
            //    string columnName = string.Empty;
            //    var columnAttr = fkColumn.GetCustomAttribute<ColumnAttribute>();
            //    if (columnAttr != null)
            //    {
            //        if (!string.IsNullOrWhiteSpace(columnAttr.Name))
            //        {
            //            columnName = columnAttr.Name;
            //        }
            //        else
            //        {
            //            columnName = fkColumn.Name;
            //        }
            //    }
            //    else
            //    {
            //        var distantKeyProp = distantProperties.FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute)));
            //        if (distantKeyProp != null)
            //        {
            //            var distantKeyAttr = distantKeyProp.GetCustomAttribute<ColumnAttribute>();
            //            columnName = string.IsNullOrWhiteSpace(distantKeyAttr?.Name)
            //                ? distantTableName + "_Id"
            //                : distantKeyAttr.Name;
            //        }
            //        else
            //        {
            //            columnName = fkColumn.Name;
            //        }
            //    }

            //    propBuilder
            //        .HasColumnName(columnName)
            //        .IsRequired(fkColumn.IsDefined(typeof(RequiredAttribute)) || (fkColumn.PropertyType.IsValueType && Nullable.GetUnderlyingType(fkColumn.PropertyType) == null));
            //    if (fkColumn.PropertyType == typeof(string) && fkColumn.IsDefined(typeof(MaxLengthAttribute)))
            //    {
            //        var lgt = fkColumn.GetCustomAttribute<MaxLengthAttribute>().Length;
            //        propBuilder.HasMaxLength(lgt);
            //        Log($"Setting maxLength of FK '{columnName}' to {lgt}", true);
            //    }
            //}
        }

        private static void ApplyColumnConstrait(EntityTypeBuilder builder, PropertyInfo column, string name, PropertyBuilder propBuilder)
        {
            if (column.IsDefined(typeof(RequiredAttribute)) || (column.PropertyType.IsValueType && Nullable.GetUnderlyingType(column.PropertyType) == null))
            {
                propBuilder.IsRequired();
                Log($"Setting '{name}' as not nullable.", true);
            }
            if (column.PropertyType == typeof(string) && column.IsDefined(typeof(MaxLengthAttribute)))
            {
                var lgt = column.GetCustomAttribute<MaxLengthAttribute>().Length;
                propBuilder.HasMaxLength(lgt);
                Log($"Setting maxLength of '{name}' to {lgt}.", true);
            }
            if (column.IsDefined(typeof(DefaultValueAttribute)))
            {
                var def = column.GetCustomAttribute<DefaultValueAttribute>().Value;
                propBuilder.HasDefaultValue(def);
                Log($"Setting defaultValue of '{name}' to {def}.", true);
            }

            StringLengthAttribute? stringLengthAttribute = column.GetCustomAttribute<StringLengthAttribute>();
            if (stringLengthAttribute != null)
            {
                propBuilder.HasMaxLength(stringLengthAttribute.MaximumLength);
            }

            DatabaseGeneratedAttribute generatedAttribute = column.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if (generatedAttribute != null)
            {
                if (generatedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Computed)
                {
                    propBuilder.ValueGeneratedOnAddOrUpdate();
                }
                else if (generatedAttribute.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                {
                    propBuilder.ValueGeneratedOnAdd();
                }
            }

            //UniqueIndexAttribute uniqueIndexAttribute = column.GetCustomAttribute<UniqueIndexAttribute>();
            //if (uniqueIndexAttribute != null)
            //{
            //    builder
            //        .HasIndex(column.Name)
            //        .IsUnique();
            //}

            ConcurrencyCheckAttribute? concurrencyCheckAttribute = column.GetCustomAttribute<ConcurrencyCheckAttribute>();

            if (concurrencyCheckAttribute != null)
            {
                propBuilder.IsConcurrencyToken(true);
            }
            TimestampAttribute? timestampAttribute = column.GetCustomAttribute<TimestampAttribute>();

            if (timestampAttribute != null)
            {
                propBuilder.IsConcurrencyToken(true).IsRowVersion();
            }
            // if (column.IsDefined(typeof(IndexAttribute)))
            // {
            //     var idxInfos = column.GetCustomAttribute<IndexAttribute>();
            //
            //     Log($"Creation of an index on column '{name}'", true);
            //     var idx = builder.HasIndex(column.Name);
            //     if (idxInfos.IsUnique)
            //     {
            //         Log(" UNIQUE", true);
            //         idx.IsUnique(true);
            //     }
            //     if (!string.IsNullOrWhiteSpace(idxInfos.IndexName))
            //     {
            //         Log($" named {idxInfos.IndexName}", true);
            //         idx.HasName(idxInfos.IndexName);
            //     }
            // }
        }

        private static void CreatePrimaryKey(EntityTypeBuilder builder, Type entityType)
        {
            var keyProps = entityType.GetProperties().Where(c => c.IsDefined(typeof(KeyAttribute))).ToArray();

            if (keyProps.Length == 0)
            {
                throw new InvalidOperationException($"Cannot retrieve primary key informations for '{entityType.Name}'");
            }

            if (keyProps.Length == 1)
            {
                var keyProp = keyProps[0];

                builder.HasKey(keyProp.Name);
            }
            else
            {
                builder.HasKey(keyProps.Select(c => c.Name).ToArray());
            }
        }

        #endregion Private static methods
    }
}