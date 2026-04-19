namespace ProjectData.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Controller { get; set; }
        public string? Action { get; set; }

        public int? PermissionId { get; set; }
        public Permission Permission { get; set; }

        public int? ParentId { get; set; }
        public MenuItem? Parent { get; set; }

        public ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
    }
}
