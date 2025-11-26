using AutoMapper;
using BookVerse.Application.Dtos.Author;
using BookVerse.Application.Dtos.Book;
using BookVerse.Application.Dtos.Category;
using BookVerse.Application.Dtos.User;
using BookVerse.Core.Entities;

namespace BookVerse.Infrastructure.Profiles;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        //Book Mapping
        CreateMap<Book, BookReadDto>()
            .ForMember(dest => dest.Authors, 
                opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author)))
            .ForMember(dest => dest.Categories, 
                opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.Category)));

        CreateMap<Book, BookBriefDto>();
        CreateMap<BookCreateDto, Book>();
        CreateMap<Book, BookReadDto>()
            .ForMember(dest => dest.Authors, 
                opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author)))
            .ForMember(dest => dest.Categories, 
                opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.Category)));
        CreateMap<BookUpdateDto, Book>();
        
        // Author Mapping
        CreateMap<Author, AuthorBriefDto>();
        CreateMap<Author, AuthorsReadDto>();
        CreateMap<Author, AuthorReadDto>()
            .ForMember(dest => dest.Books, 
                opt => opt.MapFrom(src => src.BookAuthors
                    .Select(ba => ba.Book)));
        CreateMap<AuthorCreateDto, Author>();
        CreateMap<AuthorUpdateDto, Author>();
        
        //Category Mapping
        CreateMap<CategoryCreateDto, Category>();
        CreateMap<CategoryUpdateDto, Category>();
        CreateMap<Category, CategoryReadDto>().ForMember(dest=>dest.Books,opt=>opt.MapFrom(src=>src.BookCategories.Select(bc=>bc.Book)));
        CreateMap<Category, CategoriesReadDto>();
        CreateMap<Category, CategoryBriefDto>();
        CreateMap<CategoryUpdateDto, Category>();
        
        
        CreateMap<User, UserProfileDto>();
    }
}