using MovieLibrary.BusinessLogic.Models;

namespace MovieLibrary.BusinessLogic.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetCommentsByMovieId(int id);
        Task<IEnumerable<CommentDTO>> GetCommentsByUserNickname(string nick);
        Task<int> CreateComment(CommentDTO comment);
        Task UpdateComment(CommentDTO comment);
        Task DeleteComment(int id);
    }
}
