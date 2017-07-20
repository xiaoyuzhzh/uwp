using AutoMapper;
using Sfacg.Model.ApiVO;
using Sfacg.Model.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfacg.Utils
{
    class MapperUtil
    {
        static MapperUtil()
        {
            //初始化api的映射工具
            Mapper.Initialize(cfg => {
                cfg.CreateMap<JPNovelApiVOData, Novel>();
                cfg.CreateMap<JPNovelApiVOExpand, Novel>();
                cfg.CreateMap<Data, Novel>();
                cfg.CreateMap<Expand, Novel>();
                cfg.CreateMap<SearchNovelsApiVONovels, Novel>();
                cfg.CreateMap<SearchNovelsApiVONovelsExpand, Novel>();
                cfg.CreateMap<CategoryNovelApiVOData, Novel>();
                cfg.CreateMap<CategoryNovelApiVOExpand, Novel>();
                cfg.CreateMap<PushNovelApiVOData, Novel>();
                cfg.CreateMap<VolumeList, Volume>();
                cfg.CreateMap<ChapterList, Chapter>();
            });

        }

        /// <summary>
        /// 映射方法
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="D"></typeparam>
        /// <param name="source"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public static D map<S, D>(S source, D desc)
        {
            return Mapper.Map<S, D>(source, desc);
        }
    }
}
