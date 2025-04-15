using Model.EF;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    [Required, StringLength(50)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email không được để trống!")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
    [StringLength(100)]
    public string Email { get; set; }

    [Required, StringLength(255)]
    public string PasswordHash { get; set; }

    [Required, StringLength(20)]
    public string Role { get; set; } = UserRole.User.ToString();

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ➕ Thêm UpdatedAt
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation Properties
    public virtual ICollection<UserProgress> UserProgresses { get; set; }
    public virtual ICollection<UserBadges> UserBadges { get; set; }
    public virtual ICollection<UserTopic> UserTopic { get; set; }

    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "User")]
        User = 2
    }
}
