using System;
using System.Collections.Generic;
using DiplomTwo.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomTwo.Context;

public partial class User9Context : DbContext
{
    public User9Context()
    {
    }

    public User9Context(DbContextOptions<User9Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Appauthor> Appauthors { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBookCharacter> AuthorBookCharacters { get; set; }

    public virtual DbSet<AuthorBookWorld> AuthorBookWorlds { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookChapter> BookChapters { get; set; }

    public virtual DbSet<Bookauthor> Bookauthors { get; set; }

    public virtual DbSet<Bookreview> Bookreviews { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Personallibrary> Personallibraries { get; set; }

    public virtual DbSet<Priority> Priorities { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<Readingplan> Readingplans { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Seriesbook> Seriesbooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userachievement> Userachievements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=45.67.56.214;Port=5454;Database=user9;Username=user9;Password=X8C8NTnD");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("achievements_pkey");

            entity.ToTable("achievements", "diplom");

            entity.HasIndex(e => e.Name, "achievements_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Picturename).HasColumnName("picturename");
        });

        modelBuilder.Entity<Appauthor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appauthors_pkey");

            entity.ToTable("appauthors", "diplom");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.ProfileDescription).HasColumnName("profile_description");
            entity.Property(e => e.WritingGoal)
                .HasDefaultValue(0)
                .HasColumnName("writing_goal");
            entity.Property(e => e.WrittenBooksCount)
                .HasDefaultValue(0)
                .HasColumnName("written_books_count");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Appauthor)
                .HasForeignKey<Appauthor>(d => d.Id)
                .HasConstraintName("appauthors_id_fkey");
        });

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("authors_pkey");

            entity.ToTable("authors", "diplom");

            entity.HasIndex(e => e.Name, "authors_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birth_date");
            entity.Property(e => e.DeathDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("death_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AuthorBookCharacter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_book_characters_pkey");

            entity.ToTable("author_book_characters", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Personality).HasColumnName("personality");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorBookCharacters)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("author_book_characters_book_id_fkey");
        });

        modelBuilder.Entity<AuthorBookWorld>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_book_world_pkey");

            entity.ToTable("author_book_world", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Section).HasColumnName("section");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorBookWorlds)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("author_book_world_book_id_fkey");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgeLimit).HasColumnName("age_limit");
            entity.Property(e => e.CoverImage).HasColumnName("cover_image");
            entity.Property(e => e.Dateadd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateadd");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsAuthorBook).HasColumnName("is_author_book");
            entity.Property(e => e.KolPlan).HasColumnName("kol_plan");
            entity.Property(e => e.KolRead).HasColumnName("kol_read");
            entity.Property(e => e.KolRev).HasColumnName("kol_rev");
            entity.Property(e => e.PageCount).HasColumnName("page_count");
            entity.Property(e => e.Quote).HasColumnName("quote");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("0")
                .HasColumnName("rating");
            entity.Property(e => e.SeriesId).HasColumnName("series_id");
            entity.Property(e => e.Synopsis).HasColumnName("synopsis");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Series).WithMany(p => p.Books)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("books_series_id_fkey");

            entity.HasMany(d => d.Genres).WithMany(p => p.Books)
                .UsingEntity<Dictionary<string, object>>(
                    "BookGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("book_genres_genre_id_fkey"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("book_genres_book_id_fkey"),
                    j =>
                    {
                        j.HasKey("BookId", "GenreId").HasName("book_genres_pkey");
                        j.ToTable("book_genres", "diplom");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                        j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");
                    });
        });

        modelBuilder.Entity<BookChapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_chapters_pkey");

            entity.ToTable("book_chapters", "diplom");

            entity.HasIndex(e => e.ChapterNumber, "book_chapters_chapter_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ChapterNumber).HasColumnName("chapter_number");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Book).WithMany(p => p.BookChapters)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("book_chapters_book_id_fkey");
        });

        modelBuilder.Entity<Bookauthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("bookauthors", "diplom");

            entity.Property(e => e.AppAuthorId).HasColumnName("app_author_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.AppAuthor).WithMany()
                .HasForeignKey(d => d.AppAuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookauthors_app_author_id_fkey");

            entity.HasOne(d => d.Author).WithMany()
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookauthors_author_id_fkey");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("bookauthors_book_id_fkey");
        });

        modelBuilder.Entity<Bookreview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookreviews_pkey");

            entity.ToTable("bookreviews", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.ReviewText).HasColumnName("review_text");

            entity.HasOne(d => d.Book).WithMany(p => p.Bookreviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookreviews_book_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Bookreviews)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookreviews_reader_id_fkey");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.User1Id, e.User2Id }).HasName("friends_pkey");

            entity.ToTable("friends", "diplom");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.User1Id).HasColumnName("user1_id");
            entity.Property(e => e.User2Id).HasColumnName("user2_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.User1).WithMany(p => p.FriendUser1s)
                .HasForeignKey(d => d.User1Id)
                .HasConstraintName("friends_user1_id_fkey");

            entity.HasOne(d => d.User2).WithMany(p => p.FriendUser2s)
                .HasForeignKey(d => d.User2Id)
                .HasConstraintName("friends_user2_id_fkey");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genders_pkey");

            entity.ToTable("genders", "diplom");

            entity.HasIndex(e => e.Name, "genders_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres", "diplom");

            entity.HasIndex(e => e.Name, "genres_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_pkey");

            entity.ToTable("news", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContentText).HasColumnName("content_text");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("news_user_id_fkey");
        });

        modelBuilder.Entity<Personallibrary>(entity =>
        {
            entity.HasKey(e => new { e.ReaderId, e.BookId, e.Id }).HasName("personallibrary_pkey");

            entity.ToTable("personallibrary", "diplom");

            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CharactersRating).HasColumnName("characters_rating");
            entity.Property(e => e.DateAdd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_add");
            entity.Property(e => e.Feedback).HasColumnName("feedback");
            entity.Property(e => e.HumorRating).HasColumnName("humor_rating");
            entity.Property(e => e.MeaningRating).HasColumnName("meaning_rating");
            entity.Property(e => e.PlotRating).HasColumnName("plot_rating");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RomanceRating).HasColumnName("romance_rating");
            entity.Property(e => e.WorldRating).HasColumnName("world_rating");

            entity.HasOne(d => d.Book).WithMany(p => p.Personallibraries)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("personallibrary_book_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Personallibraries)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("personallibrary_reader_id_fkey");
        });

        modelBuilder.Entity<Priority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("priority_pkey");

            entity.ToTable("priority", "diplom");

            entity.HasIndex(e => e.Name, "priority_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quotes_pkey");

            entity.ToTable("quotes", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.Text).HasColumnName("text");

            entity.HasOne(d => d.Book).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("quotes_book_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Quotes)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("quotes_reader_id_fkey");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.IdReader).HasName("readers_pkey");

            entity.ToTable("readers", "diplom");

            entity.Property(e => e.IdReader).HasColumnName("id_reader");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.ProfileDescription).HasColumnName("profile_description");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearlyGoal)
                .HasDefaultValue(0)
                .HasColumnName("yearly_goal");

            entity.HasOne(d => d.User).WithMany(p => p.Readers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("readers_user_id_fkey");

            entity.HasMany(d => d.Books).WithMany(p => p.Readers)
                .UsingEntity<Dictionary<string, object>>(
                    "Readerfavoritebook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("readerfavoritebooks_book_id_fkey"),
                    l => l.HasOne<Reader>().WithMany()
                        .HasForeignKey("ReaderId")
                        .HasConstraintName("readerfavoritebooks_reader_id_fkey"),
                    j =>
                    {
                        j.HasKey("ReaderId", "BookId").HasName("readerfavoritebooks_pkey");
                        j.ToTable("readerfavoritebooks", "diplom");
                        j.IndexerProperty<int>("ReaderId").HasColumnName("reader_id");
                        j.IndexerProperty<int>("BookId").HasColumnName("book_id");
                    });
        });

        modelBuilder.Entity<Readingplan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("readingplan_pkey");

            entity.ToTable("readingplan", "diplom");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.PriorityId).HasColumnName("priority_id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Readingplans)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("readingplan_book_id_fkey");

            entity.HasOne(d => d.Priority).WithMany(p => p.Readingplans)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("readingplan_priority_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Readingplans)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("readingplan_reader_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles", "diplom");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("series_pkey");

            entity.ToTable("series", "diplom");

            entity.HasIndex(e => e.Title, "series_title_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
        });

        modelBuilder.Entity<Seriesbook>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.SeriesId, e.BookId }).HasName("seriesbooks_pkey");

            entity.ToTable("seriesbooks", "diplom");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.SeriesId).HasColumnName("series_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.BookOrder).HasColumnName("book_order");

            entity.HasOne(d => d.Book).WithMany(p => p.Seriesbooks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("seriesbooks_book_id_fkey");

            entity.HasOne(d => d.Series).WithMany(p => p.Seriesbooks)
                .HasForeignKey(d => d.SeriesId)
                .HasConstraintName("seriesbooks_series_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "diplom");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.HasIndex(e => e.Userlastname, "users_userlastname_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .HasColumnName("login");
            entity.Property(e => e.Passwords).HasColumnName("passwords");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registration_date");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Userlastname)
                .HasMaxLength(100)
                .HasColumnName("userlastname");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Gender).WithMany(p => p.Users)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("users_gender_id_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("users_role_id_fkey");
        });

        modelBuilder.Entity<Userachievement>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ReaderId, e.AchievementId }).HasName("userachievements_pkey");

            entity.ToTable("userachievements", "diplom");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.AchievementId).HasColumnName("achievement_id");
            entity.Property(e => e.EarnedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("earned_at");

            entity.HasOne(d => d.Achievement).WithMany(p => p.Userachievements)
                .HasForeignKey(d => d.AchievementId)
                .HasConstraintName("userachievements_achievement_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Userachievements)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("userachievements_reader_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
