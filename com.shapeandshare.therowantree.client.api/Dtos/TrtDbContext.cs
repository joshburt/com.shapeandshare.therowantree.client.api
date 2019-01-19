using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace com.shapeandshare.therowantree.client.api.Dtos
{
    public partial class TrtDbContext : DbContext
    {
        public TrtDbContext()
        {
        }

        public TrtDbContext(DbContextOptions<TrtDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<Feature> Feature { get; set; }
        public virtual DbSet<FeatureState> FeatureState { get; set; }
        public virtual DbSet<FeatureType> FeatureType { get; set; }
        public virtual DbSet<FireType> FireType { get; set; }
        public virtual DbSet<IncomeSource> IncomeSource { get; set; }
        public virtual DbSet<IncomeSourceType> IncomeSourceType { get; set; }
        public virtual DbSet<MerchantTransforms> MerchantTransforms { get; set; }
        public virtual DbSet<Perk> Perk { get; set; }
        public virtual DbSet<PerkType> PerkType { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<StoreType> StoreType { get; set; }
        public virtual DbSet<TemperatureType> TemperatureType { get; set; }
        public virtual DbSet<Trapdrop> Trapdrop { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFeatureState> UserFeatureState { get; set; }
        public virtual DbSet<UserGameState> UserGameState { get; set; }
        public virtual DbSet<UserIncome> UserIncome { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("event_type", "trt_dev");

                entity.HasIndex(e => e.ActiveFeatureId)
                    .HasName("fk_feature_id_event_type_feature_type_idx");

                entity.HasIndex(e => e.EventId)
                    .HasName("event_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.EventName)
                    .HasName("event_name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FeatureStateId)
                    .HasName("fk_feature_state_id_event_type_feature_state_idx");

                entity.Property(e => e.EventId)
                    .HasColumnName("event_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActiveFeatureId)
                    .HasColumnName("active_feature_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EventDescription)
                    .HasColumnName("event_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasColumnName("event_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EventTitle)
                    .IsRequired()
                    .HasColumnName("event_title")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FeatureStateId)
                    .HasColumnName("feature_state_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ActiveFeature)
                    .WithMany(p => p.EventType)
                    .HasForeignKey(d => d.ActiveFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_feature_id_event_type_feature_type");

                entity.HasOne(d => d.ActiveFeatureNavigation)
                    .WithMany(p => p.EventType)
                    .HasForeignKey(d => d.ActiveFeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_feature_state_id_event_type_feature_state");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.HasKey(e => e.UserFeatureId);

                entity.ToTable("feature", "trt_dev");

                entity.HasIndex(e => e.FeatureId)
                    .HasName("fk_feature_id_idx");

                entity.HasIndex(e => e.UserFeatureId)
                    .HasName("user_feature_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_idx");

                entity.HasIndex(e => new { e.UserId, e.FeatureId })
                    .HasName("unique_tuples")
                    .IsUnique();

                entity.Property(e => e.UserFeatureId)
                    .HasColumnName("user_feature_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FeatureId)
                    .HasColumnName("feature_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FeatureNavigation)
                    .WithMany(p => p.Feature)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_feature_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Feature)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id");
            });

            modelBuilder.Entity<FeatureState>(entity =>
            {
                entity.ToTable("feature_state", "trt_dev");

                entity.HasIndex(e => e.FeatureId)
                    .HasName("fk_feature_id_feature_state_feature_type_idx");

                entity.HasIndex(e => e.FeatureStateId)
                    .HasName("feature_state_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.StateName)
                    .HasName("state_name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.FeatureId, e.StateIndex })
                    .HasName("fk_unique_location_index_combos")
                    .IsUnique();

                entity.Property(e => e.FeatureStateId)
                    .HasColumnName("feature_state_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FeatureId)
                    .HasColumnName("feature_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StateDescription)
                    .HasColumnName("state_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.StateIndex)
                    .HasColumnName("state_index")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StateName)
                    .HasColumnName("state_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.FeatureState)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_feature_id_feature_state_feature_type");
            });

            modelBuilder.Entity<FeatureType>(entity =>
            {
                entity.HasKey(e => e.FeatureId);

                entity.ToTable("feature_type", "trt_dev");

                entity.HasIndex(e => e.FeatureId)
                    .HasName("feature_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FeatureName)
                    .HasName("feature_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.FeatureId)
                    .HasColumnName("feature_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FeatureName)
                    .IsRequired()
                    .HasColumnName("feature_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FireType>(entity =>
            {
                entity.HasKey(e => e.FireId);

                entity.ToTable("fire_type", "trt_dev");

                entity.HasIndex(e => e.FireId)
                    .HasName("fire_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FireName)
                    .HasName("fire_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.FireId)
                    .HasColumnName("fire_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FireDescription)
                    .HasColumnName("fire_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.FireName)
                    .IsRequired()
                    .HasColumnName("fire_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IncomeSource>(entity =>
            {
                entity.HasKey(e => e.IncomeSourceStoreId);

                entity.ToTable("income_source", "trt_dev");

                entity.HasIndex(e => e.IncomeSourceId)
                    .HasName("fk_income_source_id_income_source_type_income_idx");

                entity.HasIndex(e => e.IncomeSourceStoreId)
                    .HasName("income_source_store_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.StoreId)
                    .HasName("fk_store_id_store_type_income_idx");

                entity.HasIndex(e => new { e.IncomeSourceId, e.StoreId })
                    .HasName("indx_unique_combos")
                    .IsUnique();

                entity.Property(e => e.IncomeSourceStoreId)
                    .HasColumnName("income_source_store_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.IncomeSourceId)
                    .HasColumnName("income_source_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IncomeSourceNavigation)
                    .WithMany(p => p.IncomeSource)
                    .HasForeignKey(d => d.IncomeSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_income_source_id_income_source_type_income");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.IncomeSource)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_store_id_store_type_income");
            });

            modelBuilder.Entity<IncomeSourceType>(entity =>
            {
                entity.HasKey(e => e.IncomeSourceId);

                entity.ToTable("income_source_type", "trt_dev");

                entity.HasIndex(e => e.IncomeSourceId)
                    .HasName("income_source_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IncomeSourceName)
                    .HasName("income_source_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IncomeSourceId)
                    .HasColumnName("income_source_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IncomeSourceDescription)
                    .HasColumnName("income_source_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.IncomeSourceName)
                    .IsRequired()
                    .HasColumnName("income_source_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MerchantTransforms>(entity =>
            {
                entity.ToTable("merchant_transforms", "trt_dev");

                entity.HasIndex(e => e.FromStoreId)
                    .HasName("fk_from_store_id_merchant_transforms_store_type");

                entity.HasIndex(e => e.MerchantTransformsId)
                    .HasName("merchant_transforms_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ToStoreId)
                    .HasName("fk_to_store_id_merchant_transforms_store_type_idx");

                entity.HasIndex(e => new { e.ToStoreId, e.FromStoreId })
                    .HasName("fk_unique_combos")
                    .IsUnique();

                entity.Property(e => e.MerchantTransformsId)
                    .HasColumnName("merchant_transforms_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FromStoreId)
                    .HasColumnName("from_store_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ToStoreId)
                    .HasColumnName("to_store_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FromStore)
                    .WithMany(p => p.MerchantTransformsFromStore)
                    .HasForeignKey(d => d.FromStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_from_store_id_merchant_transforms_store_type");

                entity.HasOne(d => d.ToStore)
                    .WithMany(p => p.MerchantTransformsToStore)
                    .HasForeignKey(d => d.ToStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_to_store_id_merchant_transforms_store_type");
            });

            modelBuilder.Entity<Perk>(entity =>
            {
                entity.HasKey(e => e.UserPerkId);

                entity.ToTable("perk", "trt_dev");

                entity.HasIndex(e => e.PerkId)
                    .HasName("fk_perk_id_perk_perk_type_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_perk_user_idx");

                entity.HasIndex(e => e.UserPerkId)
                    .HasName("user_perk_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserId, e.PerkId })
                    .HasName("idx_user_id_perk_id")
                    .IsUnique();

                entity.Property(e => e.UserPerkId)
                    .HasColumnName("user_perk_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerkId)
                    .HasColumnName("perk_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.PerkNavigation)
                    .WithMany(p => p.Perk)
                    .HasForeignKey(d => d.PerkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_perk_id_perk_perk_type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Perk)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_perk_user");
            });

            modelBuilder.Entity<PerkType>(entity =>
            {
                entity.HasKey(e => e.PerkId);

                entity.ToTable("perk_type", "trt_dev");

                entity.HasIndex(e => e.PerkName)
                    .HasName("perk_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.PerkId)
                    .HasColumnName("perk_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.PerkDescription)
                    .HasColumnName("perk_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.PerkName)
                    .IsRequired()
                    .HasColumnName("perk_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PerkNotify)
                    .HasColumnName("perk_notify")
                    .HasMaxLength(2048)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.UserStoreId);

                entity.ToTable("store", "trt_dev");

                entity.HasIndex(e => e.StoreId)
                    .HasName("fk_store_type_stores_user_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_idx");

                entity.HasIndex(e => e.UserStoreId)
                    .HasName("user_store_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserStoreId)
                    .HasColumnName("user_store_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.StoreNavigation)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_store_type_stores_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Store)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_stores_user");
            });

            modelBuilder.Entity<StoreType>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.ToTable("store_type", "trt_dev");

                entity.HasIndex(e => e.StoreName)
                    .HasName("store_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StoreDescription)
                    .HasColumnName("store_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasColumnName("store_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TemperatureType>(entity =>
            {
                entity.HasKey(e => e.TemperatureId);

                entity.ToTable("temperature_type", "trt_dev");

                entity.HasIndex(e => e.TemperatureId)
                    .HasName("temperature_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TemperatureName)
                    .HasName("temperature_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TemperatureId)
                    .HasColumnName("temperature_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TemperatureDescription)
                    .HasColumnName("temperature_description")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.TemperatureName)
                    .IsRequired()
                    .HasColumnName("temperature_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trapdrop>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.ToTable("trapdrop", "trt_dev");

                entity.HasIndex(e => e.StoreId)
                    .HasName("store_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.StoreId)
                    .HasColumnName("store_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RollUnder).HasColumnName("roll_under");

                entity.HasOne(d => d.Store)
                    .WithOne(p => p.Trapdrop)
                    .HasForeignKey<Trapdrop>(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_store_id_store_type_trapdrop");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "trt_dev");

                entity.HasIndex(e => e.Guid)
                    .HasName("guid_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Population)
                    .HasColumnName("population")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<UserFeatureState>(entity =>
            {
                entity.ToTable("user_feature_state", "trt_dev");

                entity.HasIndex(e => e.UserFeatureStateId)
                    .HasName("user_feature_state_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_user_feature_state_user_id_idx");

                entity.HasIndex(e => new { e.FeatureId, e.FeatureIndex })
                    .HasName("fk_feature_id_user_feature_state_feature_type_idx");

                entity.HasIndex(e => new { e.UserId, e.FeatureId })
                    .HasName("unique_user_id_feature_id_combos")
                    .IsUnique();

                entity.Property(e => e.UserFeatureStateId)
                    .HasColumnName("user_feature_state_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FeatureId)
                    .HasColumnName("feature_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.FeatureIndex)
                    .HasColumnName("feature_index")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFeatureState)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_user_feature_state_user_id");
            });

            modelBuilder.Entity<UserGameState>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("user_game_state", "trt_dev");

                entity.HasIndex(e => e.ActiveFeature)
                    .HasName("fk_feature_id_feature_idx");

                entity.HasIndex(e => e.GameFireStateId)
                    .HasName("fk_game_fire_state_id_user_game_state_fire_type_idx");

                entity.HasIndex(e => e.GameTemperatureId)
                    .HasName("fk_game_temperature_user_game_state_temperature_type_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserId, e.ActiveFeature })
                    .HasName("fk_1_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActiveFeature)
                    .HasColumnName("active_feature")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.BuilderLevel)
                    .HasColumnName("builder_level")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("-1");

                entity.Property(e => e.GameFireStateId)
                    .HasColumnName("game_fire_state_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.GameTemperatureId)
                    .HasColumnName("game_temperature_id")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.ActiveFeatureNavigation)
                    .WithMany(p => p.UserGameState)
                    .HasForeignKey(d => d.ActiveFeature)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_feature_id_feature_type_user_state");

                entity.HasOne(d => d.GameFireState)
                    .WithMany(p => p.UserGameState)
                    .HasForeignKey(d => d.GameFireStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_game_fire_state_id_user_game_state_fire_type");

                entity.HasOne(d => d.GameTemperature)
                    .WithMany(p => p.UserGameState)
                    .HasForeignKey(d => d.GameTemperatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_game_temperature_id_user_game_state_temperature_type");
            });

            modelBuilder.Entity<UserIncome>(entity =>
            {
                entity.ToTable("user_income", "trt_dev");

                entity.HasIndex(e => e.IncomeSourceId)
                    .HasName("fk_income_source_id_income_source_type_user_income_idx");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_user_user_income_idx");

                entity.HasIndex(e => e.UserIncomeId)
                    .HasName("user_income_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserId, e.IncomeSourceId })
                    .HasName("fk_unique_combos")
                    .IsUnique();

                entity.Property(e => e.UserIncomeId)
                    .HasColumnName("user_income_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IncomeSourceId)
                    .HasColumnName("income_source_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IncomeSource)
                    .WithMany(p => p.UserIncome)
                    .HasForeignKey(d => d.IncomeSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_income_source_id_income_source_type_user_income");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserIncome)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_user_user_income");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("user_info", "trt_dev");

                entity.HasIndex(e => e.UserId)
                    .HasName("user_id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Username)
                    .HasName("username_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => new { e.Username, e.Email })
                    .HasName("idx_username_email");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTime)
                    .HasColumnName("create_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserInfo)
                    .HasForeignKey<UserInfo>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_user_info_user");
            });

            modelBuilder.Entity<UserNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.ToTable("user_notification", "trt_dev");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_user_id_user_notification_user_idx");

                entity.Property(e => e.NotificationId)
                    .HasColumnName("notification_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreationDate)
                    .HasColumnName("creation_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Delivered)
                    .IsRequired()
                    .HasColumnName("delivered")
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnName("message")
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserNotification)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id_user_notification_user");
            });
        }
    }
}
