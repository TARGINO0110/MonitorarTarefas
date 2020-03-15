//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;


//namespace Monitorar_Tarefas.Controllers
//{
//    public class ArquivosController : Controller
//    {
//        public ActionResult Upload()
//        {
//            return View();
//        }

//        metodo post para realizar o upload dos arquivos em ZIP ou RAR armaznando no diretório upload.
//        GET: Arquivos
//           [HttpPost]
//            public ActionResult Upload(HttpPostedFileBase arquivoZIP)
//        {
//            try
//            {
//                var uploads = Server.MapPath("~/uploads");
//                if (arquivoZIP != null && arquivoZIP.ContentLength > 0)
//                {
//                    var allowedExtensions = new[] { ".zip", ".rar", ".pdf" };
//                    var checkextension = Path.GetExtension(arquivoZIP.FileName).ToLower();

//                    if (!allowedExtensions.Contains(checkextension))
//                    {
//                        ViewBag.Alerta = "Selecione um arquivo ZIP ou RAR menor que 20 MB";
//                        return View();
//                    }
//                    else
//                    {
//                        using (ZipArchive arquivo = new ZipArchive(arquivoZIP.InputStream))
//                        {
//                            foreach (ZipArchiveEntry entrada in arquivo.Entries)
//                            {
//                                if (!string.IsNullOrEmpty(Path.GetExtension(entrada.FullName)))
//                                {
//                                    entrada.ExtractToFile(Path.Combine(uploads, entrada.FullName));
//                                }
//                                else
//                                {
//                                    Directory.CreateDirectory(Path.Combine(uploads, entrada.FullName));
//                                }
//                            }
//                        }
//                    }

//                }

//                ViewBag.Alerta = "Arquivo ZIP enviado e descompactado com sucesso ";



//                ViewBag.Alerta = Directory.EnumerateFiles(uploads);
//                ViewData["concluido"] = "Upload realizado com sucesso!";
//                return View();

//            }
//            catch (Exception ex)
//            {
//                ViewBag.Alerta = "o Envio e a extração de arquivo ZIP / RAR falhou!" + ex.Message.ToString();
//                return View();
//            }

//        }


//        // Daqui para baixo é tratado o controlhe para realização de download
//        public ActionResult DownloadArquivos()
//        {
//            ViewBag.Arquivos = Directory.EnumerateFiles(Server.MapPath("~/uploads"));
//            return View();

//        }

//        //Efetua o download dos arquivos, aplicando uma copia ao diretorio temp
//        public ActionResult Download(List<string> arquivos)
//        {
//            try
//            {
//                if (arquivos != null)
//                {
//                    var arquivoZipado = Server.MapPath("~/arquivoZipado.zip");

//                    var temp = Server.MapPath("~/temp");

//                    if (System.IO.File.Exists(arquivoZipado))
//                    {
//                        System.IO.File.Delete(arquivoZipado);
//                    }
//                    Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

//                    arquivos.ForEach(f => System.IO.File.Copy(f, Path.Combine(temp, Path.GetFileName(f))));

//                    ZipFile.CreateFromDirectory(temp, arquivoZipado);

//                    return File(arquivoZipado, "application/zip", "arquivoZipado.zip");

//                }
//                else
//                {
//                    ViewBag.Aviso = "Selecione pelo menos um arquivo para download!";
//                    return View();
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Aviso = "Falha ao gerar e enviar o arquivo compactado!" + ex.Message.ToString();
//                return View();
//            }

//        }
//        //Efetua a remoção dos arquivos, movendo para o diretorio temp e realiza o Backup de seguanca
//        public ActionResult Deletar(List<string> arquivos)
//        {
//            try
//            {
//                if (arquivos != null)
//                {
//                    var arquivoZipado = Server.MapPath("~/backupSeguranca.zip");

//                    var temp = Server.MapPath("~/temp");

//                    if (System.IO.File.Exists(arquivoZipado))
//                    {
//                        System.IO.File.Delete(arquivoZipado);
//                    }
//                    Directory.EnumerateFiles(temp).ToList().ForEach(f => System.IO.File.Delete(f));

//                    arquivos.ForEach(f => System.IO.File.Move(f, Path.Combine(temp, Path.GetFileName(f))));

//                    ZipFile.CreateFromDirectory(temp, arquivoZipado);

//                    return File(arquivoZipado, "application/zip", "backupSeguranca.zip");

//                }
//                else
//                {
//                    ViewBag.Aviso = "Selecione pelo menos um arquivo para Exclusão!";
//                    return View();
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Aviso = "Falha ao gerar e enviar o arquivo compactado!" + ex.Message.ToString();
//                return View();
//            }

//        }
//    }
//}
//}