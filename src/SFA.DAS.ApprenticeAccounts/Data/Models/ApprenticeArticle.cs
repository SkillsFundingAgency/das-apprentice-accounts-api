using System;

namespace SFA.DAS.ApprenticeAccounts.Data.Models
{
    public class ApprenticeArticle : Entity
    {
        private ApprenticeArticle()
        {
        }

        public ApprenticeArticle(Guid id, string? entryId, bool? isSaved, bool? likeStatus)
        {
            this.Id = id;
            this.EntryId = entryId;
            this.IsSaved = isSaved;
            this.LikeStatus = likeStatus;
        }

        public Guid Id { get; set; }
        public string? EntryId { get; set; }
        public bool? IsSaved { get; set; }
        public bool? LikeStatus { get; set; }
    }
}

