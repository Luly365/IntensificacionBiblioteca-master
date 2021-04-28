﻿using IntensificacionBiblioteca.Entidades.DTOs.Pais;
using IntensificacionBiblioteca.Entidades.Entidades;
using IntensificacionBiblioteca.Servicios.Servicios;
using IntensificacionBiblioteca.Servicios.Servicios.Facades;
using IntensificacionBiblioteca.Windows.Formularios_AE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntensificacionBiblioteca.Windows
{
    public partial class PaisForm : MetroFramework.Forms.MetroForm
    {
        public PaisForm()
        {
            InitializeComponent();
        }

        private void PaisesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void SalirMetroButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        private IServiciosPais _servicio;
        private List<PaisListDto> _lista;
        private void PaisForm_Load(object sender, EventArgs e)
        {
            _servicio = new ServiciosPaises();
            try
            {
                
                _lista = _servicio.GetPaises();
                MostrardatosEnGrilla();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void MostrardatosEnGrilla()
        {
            PaisMetroGrid.Rows.Clear();
            foreach (var pais in _lista)
            {
                DataGridViewRow r = ConstruirFila();
                SetearFila(r, pais);
                AgregarFila(r);
            }
        }

        private void AgregarFila(DataGridViewRow r)
        {
            PaisMetroGrid.Rows.Add(r);
        }

        private void SetearFila(DataGridViewRow r, PaisListDto pais)
        {
            r.Cells[cmnPais.Index].Value = pais.NombrePais;
            r.Tag = pais;
        }

        private DataGridViewRow ConstruirFila()
        {
            DataGridViewRow r = new DataGridViewRow();
            r.CreateCells(PaisMetroGrid);
            return r;
        }

        private void NuevoMetroButton_Click(object sender, EventArgs e)
        {
            PaisAEForm frm = new PaisAEForm();
            frm.Text = "Nuevo Pais";
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                try
                {
                    PaisEditDto paisEditDto = frm.GetPais();

                    if (!_servicio.Existe(paisEditDto))
                    {
                        _servicio.Guardar(paisEditDto);//_servicio.Agregar(pais);
                        //var r = ConstruirFila(); aca era DataGridViewRow
                        DataGridViewRow r = ConstruirFila();
                        PaisListDto paisListDto = new PaisListDto
                        {
                            PaisId = paisEditDto.PaisId,
                            NombrePais = paisEditDto.NombrePais
                        };

                        SetearFila(r, paisListDto);
                        AgregarFila(r); //faltaba el agregarFila


                        MessageBox.Show("Registro Agregado", "Mensaje",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Registro Duplicado... Alta denegada", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void EditarMetroButton_Click(object sender, EventArgs e)
        {
            if (PaisMetroGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow r = PaisMetroGrid.SelectedRows[0];
                PaisListDto pais = (PaisListDto)r.Tag;
                PaisListDto paisAuxiliar = (PaisListDto)pais.Clone();//me hago una copia de pais
                PaisEditDto paisEditDto = new PaisEditDto
                {
                    PaisId = pais.PaisId,
                    NombrePais = pais.NombrePais
                };


                PaisAEForm frm = new PaisAEForm();
                frm.Text = "Editar Pais";
                frm.SetPais(paisEditDto);
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        paisEditDto = frm.GetPais();
                        if (!_servicio.Existe(paisEditDto))
                        {
                            _servicio.Guardar(paisEditDto);

                            pais.NombrePais = paisEditDto.NombrePais;
                            SetearFila(r, pais);
                            MessageBox.Show("Registro editado", "Mensaje", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            SetearFila(r, paisAuxiliar);

                            MessageBox.Show("Registro ya Existente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    catch (Exception exception)
                    {
                        SetearFila(r, paisAuxiliar);

                        MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BorrarMetroButton_Click(object sender, EventArgs e)
        {
            if (PaisMetroGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow r = PaisMetroGrid.SelectedRows[0];
                PaisListDto pais = (PaisListDto)r.Tag;
                DialogResult dr = MessageBox.Show($"¿Desea dar de baja al Pais {pais.NombrePais}?",
                    "Confirmar Baja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (dr == DialogResult.Yes)
                {
                    //verificar que no esta relacionado
                    try
                    {
                        _servicio.Borrar(pais.PaisId);
                        PaisMetroGrid.Rows.Remove(r);
                        MessageBox.Show("Registro Borrado", "Mensaje",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    catch (Exception exepcion)
                    {
                        MessageBox.Show(exepcion.Message, @"Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                    
                }
                
            }

        }
    }
}
