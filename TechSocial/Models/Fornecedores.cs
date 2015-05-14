using System;
using SQLite.Net.Attributes;

namespace TechSocial
{
    [Table("Fornecedores")]
    public class Fornecedores
    {
        [PrimaryKey, AutoIncrement]
        public int _Id { get; set; }

        public string fornecedor { get; set; }

        public string razaoSocial { get; set; }

        public string nomeFantasia { get; set; }

        public string categoria { get; set; }

        public string sub { get; set; }

        public string CNPJ { get; set; }

        public string inscricao { get; set; }

        public string segmento { get; set; }

        public string classe { get; set; }

        public string resp_nome { get; set; }

        public string resp_cargo { get; set; }

        public string resp_telefone { get; set; }

        public string resp_celular { get; set; }

        public string resp_email { get; set; }

        public string quadro_sede { get; set; }

        public string quadro_filiais { get; set; }

        public string quadro_emcliente { get; set; }

        public string perfil { get; set; }

        public string rua { get; set; }

        public string numero { get; set; }

        public string complemento { get; set; }

        public string bairro { get; set; }

        public string cidade { get; set; }

        public string estado { get; set; }

        public string pais { get; set; }

        public string cep9 { get; set; }

        public string tipo_documento { get; set; }

        public string nomecontato { get; set; }

        public string telefonecontato { get; set; }

        public string emailcontato { get; set; }

        public string predios { get; set; }

        public string refeitorio { get; set; }

        public string ambulatorio { get; set; }

        public string qtd_funcionarios { get; set; }

        public string nsap { get; set; }

        public string latitude { get; set; }

        public string longitude { get; set; }

        [Ignore]
        public string Endereco
        {
            get
            {
                return string.Format("{0}, {1}, {2}, {3} - {4}", this.rua, this.numero, this.bairro, this.cidade, this.estado);
            }
        }

        [Ignore]
        public string Telefones
        {
            get
            {
                return string.Format("Telefone: {0} / Celular: {1}", this.resp_telefone, this.resp_celular);
            }
        }
    }
}

