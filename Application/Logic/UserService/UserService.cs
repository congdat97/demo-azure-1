using Application.Dto.User;
using AutoMapper;
using Domain.Contracts.Group;
using Domain.Contracts.GroupPermission;
using Domain.Contracts.Permission;
using Domain.Contracts.User;
using Domain.Contracts.UserGroup;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Logic.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserJoinDto?> GetUserWithPermissionsAsync(string username)
        {
            var sql = @"
                        SELECT 
                            u.id AS UserId, u.username, u.password_hash, u.full_name, u.email,
                            g.id AS GroupId, g.group_name, g.description,
                            p.id AS PermissionId, p.permission_name, p.description
                        FROM users u
                        LEFT JOIN user_groups ug ON u.id = ug.user_id
                        LEFT JOIN groups g ON ug.group_id = g.id
                        LEFT JOIN group_permissions gp ON g.id = gp.group_id
                        LEFT JOIN permissions p ON gp.permission_id = p.id
                        WHERE u.username = @username;
                    ";

            var userDict = new Dictionary<int, UserJoinDto>();

            var userResult = await _unitOfWork.GetRepository<UserJoinEntity>().GetDataAsync(sql, new { username });

            foreach (var row in userResult.ToList())
            {
                if (!userDict.TryGetValue(row.UserId, out var userEntry))
                {
                    userEntry = new UserJoinDto
                    {
                        Id = row.UserId,
                        Username = row.Username,
                        PasswordHash = row.Password_Hash,
                        FullName = row.Full_Name,
                        Email = row.Email,
                        UserGroups = new List<UserGroupDto>()
                    };
                    userDict[userEntry.Id] = userEntry;
                }

                if (row.GroupId.HasValue)
                {
                    var group = new GroupDto
                    {
                        Id = row.GroupId.Value,
                        GroupName = row.Group_Name!,
                        Description = row.Group_Description,
                        GroupPermissions = new List<GroupPermissionDto>()
                    };

                    var userGroup = userEntry.UserGroups.FirstOrDefault(x => x.GroupId == group.Id);
                    if (userGroup == null)
                    {
                        userEntry.UserGroups.Add(new UserGroupDto
                        {
                            UserId = userEntry.Id,
                            GroupId = group.Id,
                            Group = group
                        });
                    }

                    if (row.PermissionId.HasValue)
                    {
                        if (!group.GroupPermissions.Any(x => x.PermissionId == row.PermissionId))
                        {
                            group.GroupPermissions.Add(new GroupPermissionDto
                            {
                                GroupId = group.Id,
                                PermissionId = row.PermissionId.Value,
                                Group = group,
                                Permission = new PermissionDto
                                {
                                    Id = row.PermissionId.Value,
                                    PermissionName = row.Permission_Name!,
                                    Description = row.Permission_Description
                                }
                            });
                        }
                    }
                }
            }

            var user = userDict.Values.FirstOrDefault();
            return user;
        }
    }
}
