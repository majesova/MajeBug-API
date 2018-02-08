using AutoMapper;
using MajeBug.Domain;
using MajeBugWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MajeBugWebApi.Helpers
{
    public class MapperHelper
    {
        internal static IMapper mapper;
        static MapperHelper()
        {

            var config = new MapperConfiguration(x=> {

                x.CreateMap<Bug, BugApi>().ReverseMap();
                x.CreateMap<User, UserApi>().ReverseMap();
                x.CreateMap<Bug, CreateBugApi>().ReverseMap();

            });
            mapper = config.CreateMapper();
        }

        public static T Map<T>(object source) {
            return mapper.Map<T>(source);
        }

    }
}