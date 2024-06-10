using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using System.Data;

namespace Data_Access_Layer
{
    public class DALLogin
    {
        private readonly AppDbContext _cIDbContext;
        public DALLogin(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        public User GetUserById(int userId)
        {
            try
            {      User user = new User();
                    // Retrieve the user by ID
                    user = _cIDbContext.User.FirstOrDefault(u => u.Id == userId && !u.IsDeleted);
                    if (user != null)
                    {
                        return user;
                    }
                    else
                    {
                        throw new Exception("User not found.");
                    }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Register(User user)
        {
            string result = "";
            try
            {
                // Check if the email address already exists
                bool emailExists = _cIDbContext.User.Any(u => u.EmailAddress == user.EmailAddress && !u.IsDeleted);

                if (!emailExists)
                {
                    string maxEmployeeIdStr = _cIDbContext.UserDetail.Max(ud => ud.EmployeeId);
                     
                    int maxIdStr = (int)_cIDbContext.User.Max(ud => ud.Id);
                    
                    int maxEmployeeId = 0;

                    // Convert the maximum EmployeeId to an integer
                    if (!string.IsNullOrEmpty(maxEmployeeIdStr))
                    {
                        if (int.TryParse(maxEmployeeIdStr, out int parsedEmployeeId))
                        {
                            maxEmployeeId = parsedEmployeeId;
                        }
                        else
                        {
                            // Handle conversion error
                            throw new Exception("Error converting EmployeeId to integer.");
                        }
                    }
                    int newEmployeeId = maxEmployeeId + 1;

                    int maxId = 0;
                    if (maxIdStr > maxId)
                    {
                        maxId = maxIdStr;
                    }
                    int newmaxId = maxId + 1;

                    var newUser = new User
                    {
                        Id = newmaxId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        Password = user.Password,
                        UserType = user.UserType,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false

                    };
                    var newUserDetail = new UserDetail
                    {
                        UserId = newmaxId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        UserType = user.UserType,
                        Name = user.FirstName,
                        Surname = user.LastName,
                        EmployeeId = newEmployeeId.ToString(),
                        Department = "IT",
                        Status = true,
                        CreatedDate = DateTime.UtcNow
                    };
                    _cIDbContext.User.Add(newUser);
                    _cIDbContext.UserDetail.Add(newUserDetail);
                    _cIDbContext.SaveChanges();

                    result = "User register successfully.";
                }
                else
                {
                    throw new Exception("Email Address Already Exist.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        public string UpdateUser(User updatedUser)
        {
            string result = "";
            try
            {
                var existingUser = _cIDbContext.User.FirstOrDefault(u => u.Id == updatedUser.Id && !u.IsDeleted);
                var existingUserDetail = _cIDbContext.UserDetail.FirstOrDefault(u => u.UserId == updatedUser.Id && !u.IsDeleted);

                if (existingUser != null && existingUserDetail != null)
                {
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.PhoneNumber = updatedUser.PhoneNumber;
                    existingUser.EmailAddress = updatedUser.EmailAddress; // Add this line
                    existingUser.ModifiedDate = DateTime.UtcNow;
                    existingUserDetail.FirstName = updatedUser.FirstName;
                    existingUserDetail.LastName = updatedUser.LastName;
                    existingUserDetail.PhoneNumber = updatedUser.PhoneNumber;
                    existingUserDetail.EmailAddress = updatedUser.EmailAddress;
                    existingUserDetail.Name = updatedUser.FirstName;
                    existingUserDetail.Surname = updatedUser.LastName;
                    existingUserDetail.ModifiedDate = DateTime.UtcNow;
                    _cIDbContext.SaveChanges();

                    result = "User updated successfully.";
                }
                else
                {
                    throw new Exception("User not found or already deleted.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }




        public User LoginUser(User user)
        {
            User userObj = new User();
            try
            {
                    var query = from u in _cIDbContext.User
                                where u.EmailAddress == user.EmailAddress && u.IsDeleted == false
                                select new
                                {
                                    u.Id,
                                    u.FirstName,
                                    u.LastName,
                                    u.PhoneNumber,
                                    u.EmailAddress,
                                    u.UserType,
                                    u.Password,
                                    UserImage = u.UserImage
                                };

                    var userData = query.FirstOrDefault();
                    if (userData != null)
                    {
                        if (userData.Password == user.Password)
                        {
                            userObj.Id = userData.Id;
                            userObj.FirstName = userData.FirstName;
                            userObj.LastName = userData.LastName;
                            userObj.PhoneNumber = userData.PhoneNumber;
                            userObj.EmailAddress = userData.EmailAddress;
                            userObj.UserType = userData.UserType;
                            userObj.UserImage = userData.UserImage;
                            userObj.Message = "Login Successfully";
                        }
                        else
                        {
                            userObj.Message = "Incorrect Password.";
                        }
                    }
                    else
                    {
                        userObj.Message = "Email Address Not Found.";
                    }
            }
            catch (Exception)
            {
                throw;
            }
            return userObj;
        }
        public string LoginUserProfileUpdate(UserDetail userDetail)
        {
            string result = "";
            try
            {
                // Start a transaction
                using (var transaction = _cIDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Check if the user exists
                        var existingUserDetail = _cIDbContext.UserDetail
                            .FirstOrDefault(u => u.UserId == userDetail.UserId && !u.IsDeleted);
                        if (existingUserDetail != null)
                        {
                            // Update the existing user detail
                            existingUserDetail.Name = userDetail.Name;
                            existingUserDetail.Surname = userDetail.Surname;
                            existingUserDetail.EmployeeId = userDetail.EmployeeId;
                            existingUserDetail.Manager = userDetail.Manager;
                            existingUserDetail.Title = userDetail.Title;
                            existingUserDetail.Department = userDetail.Department;
                            existingUserDetail.MyProfile = userDetail.MyProfile;
                            existingUserDetail.WhyIVolunteer = userDetail.WhyIVolunteer;
                            existingUserDetail.CountryId = userDetail.CountryId;
                            existingUserDetail.CityId = userDetail.CityId;
                            existingUserDetail.Avilability = userDetail.Avilability;
                            existingUserDetail.LinkdInUrl = userDetail.LinkdInUrl;
                            existingUserDetail.MySkills = userDetail.MySkills;
                            existingUserDetail.UserImage = userDetail.UserImage;
                            existingUserDetail.Status = userDetail.Status;
                            existingUserDetail.ModifiedDate = DateTime.UtcNow;
                            result = "Account Updated Successfully...";
                        }
                        else
                        {
                            // Insert new user detail
                            userDetail.CreatedDate = DateTime.Now;
                            userDetail.ModifiedDate = null;
                            userDetail.IsDeleted = false;
                            _cIDbContext.UserDetail.Add(userDetail);
                            result = "Account Created Successfully...";
                        }
                        _cIDbContext.SaveChanges();
                        // Update the User table
                        var user = _cIDbContext.User.FirstOrDefault(u => u.Id == userDetail.UserId);
                        if (user != null)
                        {
                            user.FirstName = userDetail.Name;
                            user.LastName = userDetail.Surname;
                        }
                        _cIDbContext.SaveChanges();
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction if something goes wrong
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public User LoginUserDetailById(int id)
        {
            User userDetail = new User();
            try
            {
                userDetail = _cIDbContext.User.FirstOrDefault(u => u.Id == id && !u.IsDeleted);
            }
            catch (Exception)
            {
                throw;
            }
            return userDetail;
        }
        public UserDetail GetUserProfileDetailById(int userId)
        {
            UserDetail userDetail = new UserDetail();
            try
            {
                userDetail = _cIDbContext.UserDetail.FirstOrDefault(u => u.UserId == userId && !u.IsDeleted);
            }
            catch (Exception)
            {
                throw;
            }
            return userDetail;
        }
    }
}
