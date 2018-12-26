using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace com.shapeandshare.therowantree.client.api.trt
{
    public partial class trt_devContext : DbContext
    {
        public trt_devContext()
        {
        }

        public trt_devContext(DbContextOptions<trt_devContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EventType> EventType { get; set; }
        public virtual DbSet<FeatureState> FeatureState { get; set; }
        public virtual DbSet<FeatureType> FeatureType { get; set; }
        public virtual DbSet<FireType> FireType { get; set; }
        public virtual DbSet<IncomeSourceType> IncomeSourceType { get; set; }
        public virtual DbSet<PerkType> PerkType { get; set; }
        public virtual DbSet<StoreType> StoreType { get; set; }
        public virtual DbSet<TemperatureType> TemperatureType { get; set; }
        public virtual DbSet<Trapdrop> Trapdrop { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGameState> UserGameState { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserNotification> UserNotification { get; set; }

        // Unable to generate entity type for table 'trt_dev.feature'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.income_source'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.merchant_transforms'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.perk'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.store'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.user_feature_state'. Please see the warning messages.
        // Unable to generate entity type for table 'trt_dev.user_income'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=1ZHvG9leSgJdOvFZR1PkXSOo6xrPCNrNC1q6EWp1YUOnEERNkFdrXzNnW2k3SAf;database=trt_dev");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
