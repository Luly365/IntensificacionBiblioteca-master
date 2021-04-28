﻿using IntensificacionBiblioteca.Entidades.DTOs.Genero;
using IntensificacionBiblioteca.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntensificacionBiblioteca.Windows.Formularios_AE
{
    public partial class GeneroAEForm : MetroFramework.Forms.MetroForm
    {
        public GeneroAEForm()
        {
            InitializeComponent();
        }

        private void CancelMetroButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private GeneroEditDto genero;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (genero != null)
            {
                GeneroMetroTextBox.Text = genero.Descripcion;
            }
        }

        public GeneroEditDto GetGenero()
        {
            return genero;
        }

        private void OKMetroButton_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                if (genero == null)
                {
                    genero = new GeneroEditDto();
                }

                genero.Descripcion = GeneroMetroTextBox.Text.Trim();//trip
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            if (string.IsNullOrEmpty(GeneroMetroTextBox.Text.Trim()))
            {
                valido = false;
                errorProvider1.SetError(GeneroMetroTextBox, "Debe ingresar un Genero");
            }

            return valido;
        }

        public void SetGenero(GeneroEditDto genero)
        {
            this.genero = genero;
        }

        private void GeneroAEForm_Load(object sender, EventArgs e)
        {

        }
    }
}
