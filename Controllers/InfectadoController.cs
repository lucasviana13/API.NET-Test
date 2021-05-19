using System;
using Api_Teste.Data.Collections;
using Api_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Api_Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    
    public class InfectadoController : ControllerBase
    {
        Data.Mongo _mongo;
        IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(Data.Mongo mongoDB)
        {
            _mongo = mongoDB;
            _infectadosCollection = _mongo.DB.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public ActionResult SalvarInfectado([FromBody] InfectadoDto dto)
        {
            var infectado = new Infectado(dto.dataNascimento, dto.sexo, dto.latitude, dto.longitude);

            _infectadosCollection.InsertOne(infectado);
            return StatusCode(201, "Adicionado um novo Infectado");
        }

        [HttpGet]
        public ActionResult ObterInfectados()
        {
            var infectados = _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToList();
            
            return Ok(infectados); //HTTP status 200
        }

        [HttpPut]
        public ActionResult AtualizarInfectado([FromBody] InfectadoDto dto)
        {  
            _infectadosCollection.UpdateOne(Builders<Infectado>.Filter.Where(_ =>_.DataNascimento == dto.dataNascimento), Builders<Infectado>.Update.Set("sexo",dto.sexo));
            //Altera o sexo
            return Ok("Atualizado com sucesso");
        }

        [HttpDelete("{nascimento}")]
        public ActionResult Delete(DateTime nascimento)
        {  
            _infectadosCollection.DeleteOne(Builders<Infectado>.Filter.Where(_=>_.DataNascimento == nascimento));

            return Ok("Deletado com sucesso");
        }


    }
}