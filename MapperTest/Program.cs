using AutoMapper;
using BenchmarkDotNet.Running;
using Nelibur.ObjectMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var userList = new List<User>();

            for (var i = 0; i < 10; i++)
            {
                userList.Add(new User
                {
                    Address = Faker.Address.SecondaryAddress(),
                    City = Faker.Address.City(),
                    Age = Faker.RandomNumber.Next(10, 100),
                    Name = Faker.Name.First(),
                    Surname = Faker.Name.Last(),
                    Phone = Faker.Phone.Number(),
                    Web = Faker.Internet.DomainName(),
                    Email = Faker.Internet.Email(),
                    Country = Faker.Address.Country()
                });
            }

            #region Initialize
            #region AutoMapper
            var configAutoMapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDto>());
            AutoMapper.Mapper.Initialize(cfg => cfg.CreateMap<User, UserDto>());
            #endregion
            #endregion


            var userDtoList = new List<UserDto>();

            #region AutoMapper
            var createAutoMapper = configAutoMapper.CreateMapper();
            var dtoAutoMapper = userList.Select(T => createAutoMapper.Map<UserDto>(T)).ToList();
            #endregion

            #region LinqMapper
            userDtoList.Clear();
            userDtoList.AddRange(userList.Select(T => new UserDto
            {
                Name = T.Name,
                Surname = T.Surname
            }).ToList());
            #endregion

            #region TinyMapper
            userDtoList.Clear();
            foreach (var user in userList)
            {
                userDtoList.Add(TinyMapper.Map<UserDto>(user));
            }
            #endregion

            #region ExpressMapper
            userDtoList.Clear();
            foreach (var user in userList)
            {
                userDtoList.Add(ExpressMapper.Mapper.Map<User, UserDto>(user));
            }
            #endregion

            #region Benchmarking
            BenchmarkRunner.Run<MapperBenchmarking>();
            #endregion
        }
    }
}
