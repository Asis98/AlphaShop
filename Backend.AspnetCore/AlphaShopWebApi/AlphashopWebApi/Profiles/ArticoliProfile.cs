using AlphashopWebApi.Dtos;
using AlphashopWebApi.Models;
using AutoMapper;

namespace AlphashopWebApi.Profiles
{
    public class ArticoliProfile : Profile
    {
        public ArticoliProfile()
        {
            CreateMap<Articoli, ArticoliDto>()
                .ForMember(
                    dest => dest.Categoria,
                    opt => opt.MapFrom(src => $"{src.IdFamAss} {src.familyAssort!.Descrizione}")
                )
                .ForMember(dest => dest.CodStat, opt => opt.MapFrom(src => src.CodStat!.Trim()))
                .ForMember(dest => dest.Um, opt => opt.MapFrom(src => src.Um!.Trim()))
                .ForMember(
                    dest => dest.IdStatoArticolo,
                    opt => opt.MapFrom(src => src.IdStatoArt!.Trim())
                )
                .ForMember(
                    dest => dest.Iva,
                    opt => opt.MapFrom(src => new IvaDto(src.iva!.Descrizione!, src.iva.Aliquota))
                )
                .ForMember(
                    dest => dest.PzCart,
                    opt => opt.MapFrom(src => src.PzCart == null ? 0 : src.PzCart)
                );

            CreateMap<Iva, IvaDto>();
            CreateMap<FamilyAssort, CategoryDto>();
        }
    }
}
