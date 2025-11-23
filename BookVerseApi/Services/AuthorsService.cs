using AutoMapper;
using BookVerseApi.Dtos.Author;
using BookVerseApi.Entities;
using BookVerseApi.Interfaces;

namespace BookVerseApi.Services;

public class AuthorsService:IAuthorsService
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;

    public AuthorsService(IAuthorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AuthorsReadDto>> GetAllAsync()
    {
        var authors = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<AuthorsReadDto>>(authors);
    }

    public async Task<AuthorReadDto?> GetByIdAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id);
        return _mapper.Map<AuthorReadDto>(author);
    }

    public async Task<AuthorReadDto> CreateAsync(AuthorCreateDto authorDto)
    {
        var author = _mapper.Map<Author>(authorDto);
        var existingAuthor = await _repository.GetByNameAsync(author.FirstName, author.LastName);

        if (existingAuthor != null)
        {
            return _mapper.Map<AuthorReadDto>(existingAuthor);
        }
        else
        {
            // author.CreatedAtUtc = DateTime.UtcNow;
            // author.UpdatedAtUtc = DateTime.UtcNow;
            await _repository.AddAsync(author);
            await _repository.SaveAsync();
            return _mapper.Map<AuthorReadDto>(author);
        }
        
    }

    public async Task<bool> UpdateAsync(int id, AuthorUpdateDto authorDto)
    {
        var retrievedAuthor = await _repository.GetByIdAsync(id);
        if (retrievedAuthor == null) return false;
        
         _mapper.Map(authorDto,retrievedAuthor);
         // retrievedAuthor.UpdatedAtUtc = DateTime.UtcNow;
        _repository.Update(retrievedAuthor);
        await _repository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var retrievedAuthor = await _repository.GetByIdAsync(id);
        if (retrievedAuthor == null) return false;
        
        _repository.Delete(retrievedAuthor);
        await _repository.SaveAsync();
        return true;
    }
}