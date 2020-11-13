using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using StraviaTec_Web.Models;
using StraviaTec_Web.Models.Dtos;

namespace StraviaTec_Web.Helpers
{
    public class StraviaTecProfile: Profile
    {
        public StraviaTecProfile()
        {
            //Insertar mapeo de datos de una clase a otra
            CreateMap<Carrera, CarreraDto>().IncludeMembers(c => c.IdEventoNavigation);
            CreateMap<Evento, CarreraDto>(MemberList.None);
            CreateMap<TipoActividad, TipoActividadDto>();
            CreateMap<Patrocinador, PatrocinadorDto>();
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<CategoriaCarrera, CategoriaCarreraDto>();
            CreateMap<EventoGrupo, EventoGrupoDto>();
            CreateMap<PatrocinadorEvento, PatrocinadorEventoDto>();
            CreateMap<CuentaBancaria, CuentaBancariaDto>();
            CreateMap<TipoReto, TipoRetoDto>();
        }
    }
}