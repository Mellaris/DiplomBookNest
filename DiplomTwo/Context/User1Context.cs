using System;
using System.Collections.Generic;
using DiplomTwo.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomTwo.Context;

public partial class User1Context : DbContext
{
    public User1Context()
    {
    }

    public User1Context(DbContextOptions<User1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Appauthor> Appauthors { get; set; }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBookCharacter> AuthorBookCharacters { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookChapter> BookChapters { get; set; }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<Bookauthor> Bookauthors { get; set; }

    public virtual DbSet<Bookplan> Bookplans { get; set; }

    public virtual DbSet<Bookreview> Bookreviews { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ElectronicBooksInfo> ElectronicBooksInfos { get; set; }

    public virtual DbSet<Friendrelation> Friendrelations { get; set; }

    public virtual DbSet<Friendrequeststatus> Friendrequeststatuses { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Personallibrary> Personallibraries { get; set; }

    public virtual DbSet<Prioritylevel> Prioritylevels { get; set; }

    public virtual DbSet<Publicaccesslevel> Publicaccesslevels { get; set; }

    public virtual DbSet<Quote> Quotes { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<Readerfavoritebook> Readerfavoritebooks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<Seriesbook> Seriesbooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userachievement> Userachievements { get; set; }

    public virtual DbSet<VerificationCode> VerificationCodes { get; set; }

    public virtual DbSet<WorldSection> WorldSections { get; set; }

    public virtual DbSet<WorldSectionContent> WorldSectionContents { get; set; }

    public virtual DbSet<WritingStatus> WritingStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=45.67.56.214;Port=5666;Database=user1;Username=user1;Password=Xgny6RrJ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("achievements_pkey");

            entity.ToTable("achievements");

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

            entity.ToTable("appauthors");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.ProfileDescription).HasColumnName("profile_description");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registration_date");
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

            entity.ToTable("authors");

            entity.HasIndex(e => e.Name, "authors_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Biography).HasColumnName("biography");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.DeathDate).HasColumnName("death_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AuthorBookCharacter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("author_book_characters_pkey");

            entity.ToTable("author_book_characters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Personality).HasColumnName("personality");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorBookCharacters)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("author_book_characters_book_id_fkey");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("books_pkey");

            entity.ToTable("books");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgeLimit).HasColumnName("age_limit");
            entity.Property(e => e.CoverImage).HasColumnName("cover_image");
            entity.Property(e => e.Dateadd)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateadd");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsAuthorBook).HasColumnName("is_author_book");
            entity.Property(e => e.Kolplan).HasColumnName("kolplan");
            entity.Property(e => e.Kolread).HasColumnName("kolread");
            entity.Property(e => e.Kolrev).HasColumnName("kolrev");
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
        });

        modelBuilder.Entity<BookChapter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("book_chapters_pkey");

            entity.ToTable("book_chapters");

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
            entity.Property(e => e.WordCount).HasColumnName("word_count");

            entity.HasOne(d => d.Book).WithMany(p => p.BookChapters)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("book_chapters_book_id_fkey");
        });

        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.BookId, e.GenreId }).HasName("book_genres_pkey");

            entity.ToTable("book_genres");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");

            entity.HasOne(d => d.Book).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("book_genres_book_id_fkey");

            entity.HasOne(d => d.Genre).WithMany(p => p.BookGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("book_genres_genre_id_fkey");
        });

        modelBuilder.Entity<Bookauthor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookauthors_pkey");

            entity.ToTable("bookauthors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppAuthorId).HasColumnName("app_author_id");
            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.AppAuthor).WithMany(p => p.Bookauthors)
                .HasForeignKey(d => d.AppAuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookauthors_app_author_id_fkey");

            entity.HasOne(d => d.Author).WithMany(p => p.Bookauthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bookauthors_author_id_fkey");

            entity.HasOne(d => d.Book).WithMany(p => p.Bookauthors)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("bookauthors_book_id_fkey");
        });

        modelBuilder.Entity<Bookplan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookplan_pkey");

            entity.ToTable("bookplan");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.PriorityId).HasColumnName("priority_id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Bookplans)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("fk_book");

            entity.HasOne(d => d.Priority).WithMany(p => p.Bookplans)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_priority");

            entity.HasOne(d => d.Reader).WithMany(p => p.Bookplans)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("fk_reader");
        });

        modelBuilder.Entity<Bookreview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bookreviews_pkey");

            entity.ToTable("bookreviews");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsHaveRev).HasColumnName("is_have_rev");
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

        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("characters_pkey");

            entity.ToTable("characters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Ethnicity).HasColumnName("ethnicity");
            entity.Property(e => e.Fears).HasColumnName("fears");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.Goal).HasColumnName("goal");
            entity.Property(e => e.Hobbies).HasColumnName("hobbies");
            entity.Property(e => e.ImageName).HasColumnName("image_name");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Occupation).HasColumnName("occupation");
            entity.Property(e => e.Personality).HasColumnName("personality");
            entity.Property(e => e.Strengths).HasColumnName("strengths");
            entity.Property(e => e.Weaknesses).HasColumnName("weaknesses");

            entity.HasOne(d => d.Book).WithMany(p => p.Characters)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("fk_character_book");

            entity.HasOne(d => d.Gender).WithMany(p => p.Characters)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_character_gender");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ChapterId).HasColumnName("chapter_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Comments)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("comments_book_id_fkey");

            entity.HasOne(d => d.Chapter).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ChapterId)
                .HasConstraintName("comments_chapter_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("comments_reader_id_fkey");
        });

        modelBuilder.Entity<ElectronicBooksInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("electronic_books_info_pkey");

            entity.ToTable("electronic_books_info");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessLevelId).HasColumnName("access_level_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.ChapterCount).HasColumnName("chapter_count");
            entity.Property(e => e.CurrentWordCount).HasColumnName("current_word_count");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EstimatedPageCount).HasColumnName("estimated_page_count");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TargetWordCount).HasColumnName("target_word_count");

            entity.HasOne(d => d.AccessLevel).WithMany(p => p.ElectronicBooksInfos)
                .HasForeignKey(d => d.AccessLevelId)
                .HasConstraintName("fk_access_level");

            entity.HasOne(d => d.Book).WithMany(p => p.ElectronicBooksInfos)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("electronic_books_info_book_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.ElectronicBooksInfos)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("electronic_books_info_status_id_fkey");
        });

        modelBuilder.Entity<Friendrelation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendrelations_pkey");

            entity.ToTable("friendrelations");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fromuserid).HasColumnName("fromuserid");
            entity.Property(e => e.Requestdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("requestdate");
            entity.Property(e => e.Statusid).HasColumnName("statusid");
            entity.Property(e => e.Touserid).HasColumnName("touserid");

            entity.HasOne(d => d.Fromuser).WithMany(p => p.FriendrelationFromusers)
                .HasForeignKey(d => d.Fromuserid)
                .HasConstraintName("friendrelations_fromuserid_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Friendrelations)
                .HasForeignKey(d => d.Statusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendrelations_statusid_fkey");

            entity.HasOne(d => d.Touser).WithMany(p => p.FriendrelationTousers)
                .HasForeignKey(d => d.Touserid)
                .HasConstraintName("friendrelations_touserid_fkey");
        });

        modelBuilder.Entity<Friendrequeststatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("friendrequeststatuses_pkey");

            entity.ToTable("friendrequeststatuses");

            entity.HasIndex(e => e.Name, "friendrequeststatuses_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genders_pkey");

            entity.ToTable("genders");

            entity.HasIndex(e => e.Name, "genders_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.Name, "genres_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("news_pkey");

            entity.ToTable("news");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.ContentText).HasColumnName("content_text");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
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

            entity.ToTable("personallibrary");

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

        modelBuilder.Entity<Prioritylevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("prioritylevel_pkey");

            entity.ToTable("prioritylevel");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Publicaccesslevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("access_levels_pkey");

            entity.ToTable("publicaccesslevels");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quotes_pkey");

            entity.ToTable("quotes");

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

            entity.ToTable("readers");

            entity.Property(e => e.IdReader).HasColumnName("id_reader");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.ProfileDescription).HasColumnName("profile_description");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearlyGoal)
                .HasDefaultValue(0)
                .HasColumnName("yearly_goal");

            entity.HasOne(d => d.User).WithMany(p => p.Readers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("readers_user_id_fkey");
        });

        modelBuilder.Entity<Readerfavoritebook>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ReaderId, e.BookId }).HasName("readerfavoritebooks_pkey");

            entity.ToTable("readerfavoritebooks");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.Book).WithMany(p => p.Readerfavoritebooks)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("readerfavoritebooks_book_id_fkey");

            entity.HasOne(d => d.Reader).WithMany(p => p.Readerfavoritebooks)
                .HasForeignKey(d => d.ReaderId)
                .HasConstraintName("readerfavoritebooks_reader_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "roles_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("series_pkey");

            entity.ToTable("series");

            entity.HasIndex(e => e.Title, "series_title_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Series)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_series_user");
        });

        modelBuilder.Entity<Seriesbook>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.SeriesId, e.BookId }).HasName("seriesbooks_pkey");

            entity.ToTable("seriesbooks");

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

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

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

            entity.ToTable("userachievements");

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

        modelBuilder.Entity<VerificationCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("verification_codes_pkey");

            entity.ToTable("verification_codes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.VerificationCodes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<WorldSection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("world_sections_pkey");

            entity.ToTable("world_sections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ShortDescription).HasColumnName("short_description");
        });

        modelBuilder.Entity<WorldSectionContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("world_section_contents_pkey");

            entity.ToTable("world_section_contents");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.WorldSectionId).HasColumnName("world_section_id");

            entity.HasOne(d => d.Book).WithMany(p => p.WorldSectionContents)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("fk_content_book");

            entity.HasOne(d => d.WorldSection).WithMany(p => p.WorldSectionContents)
                .HasForeignKey(d => d.WorldSectionId)
                .HasConstraintName("fk_content_section");
        });

        modelBuilder.Entity<WritingStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("writing_statuses_pkey");

            entity.ToTable("writing_statuses");

            entity.HasIndex(e => e.Name, "writing_statuses_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
