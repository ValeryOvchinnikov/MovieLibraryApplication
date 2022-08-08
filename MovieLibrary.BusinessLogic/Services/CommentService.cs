using AutoMapper;
using MovieLibrary.BusinessLogic.Infrastructure;
using MovieLibrary.BusinessLogic.Infrastructure.Logger;
using MovieLibrary.BusinessLogic.Interfaces;
using MovieLibrary.BusinessLogic.Models;
using MovieLibrary.DataAccess.Interfaces;
using MovieLibrary.DataAccess.Models;

namespace MovieLibrary.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private ILog _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(IMovieRepository movieRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            _logger = Log.GetInstance;
            _movieRepository = movieRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateComment(CommentDTO comment)
        {
            if (comment == null)
            {
                throw new ValidationException("Data not found");
            }

            var mappedComment = _mapper.Map<Comment>(comment);

            Movie? movie = await _movieRepository.GetByIdAsync(comment.MovieId);

            if (movie == null)
            {
                throw new ValidationException("Movie was not found");
            }
            await _commentRepository.CreateAsync(mappedComment);
            return comment.MovieId;
        }

        public async Task DeleteComment(int id)
        {
            if (await _commentRepository.GetByIdAsync(id) == null)
            {
                throw new ValidationException("Comment was not found");
            }

            await _commentRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByMovieId(int id)
        {
            List<Comment>? comments = await _commentRepository.GetAllByMovieId(id) as List<Comment>;

            IEnumerable<CommentDTO> mappedComments = comments!.Select(comment => _mapper.Map<CommentDTO>(comment));
            return mappedComments;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByUserNickname(string nick)
        {
            List<Comment>? comments = await _commentRepository.GetAllByUserNickname(nick) as List<Comment>;

            IEnumerable<CommentDTO> mappedComments = comments!.Select(comment => _mapper.Map<CommentDTO>(comment));
            return mappedComments;
        }

        public async Task UpdateComment(CommentDTO comment)
        {
            if (comment == null)
            {
                throw new ValidationException("Data not found");
            }

            var oldComment = await _commentRepository.GetByIdAsync(comment.Id);

            if (oldComment == null)
            {
                throw new ValidationException("Comment not found");
            }

            var newComment = _mapper.Map<Comment>(comment);
            await _commentRepository.UpdateAsync(newComment);
        }
    }
}
