using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using EuLum.net.Models;
using EuLum.net.Models.Enums;
using Elumdat = EuLum.net.Models.Raw.Elumdat;

namespace EuLum.net.net
{
    class Program
    {
        static void Main()
        {
            using (var sr = new StreamReader("test.ldt"))
            {
                var contents = sr.ReadToEnd().Split('\n').ToList();
                var elumdat = new Elumdat(contents);

                var general = SetupGeneral(elumdat);
                var luminaire = SetupLuminaire(elumdat);
                var lampSets = SetupLamps(elumdat);
                var utilizationFactors = SetupUtilizationFactors(elumdat);
                var luminousIntensity = elumdat.DLcd;

                var elumDat = new Models.Elumdat(general, luminaire, lampSets, utilizationFactors, luminousIntensity);
                Console.WriteLine(elumDat);
                var bitmapOne = new Bitmap(640, 640);

                for (var i = 0; i < 640; i++)
                {
                    for (var j = 0; j < 640; j++)
                    {
                        bitmapOne.SetPixel(i, j, Color.Black);
                    }
                }

                var bitmapTwo = new Bitmap(640, 640);

                SetVectors(elumdat, 640, 640);
            }
        }

        private static void SetVectors(Elumdat elumdat, int width, int height)
        {
            var one = new List<Point>();
            var two = new List<Point>();

            //void setVectors()
            //{	
            //	one.clear();
            //	two.clear();
            //	
            //	int a, b;    
            //    double x=0, y=0;    
            //   	max = 0;   
            int a, b;
            double x = 0, y = 0, max = 0;


            //    // C0
            //    if(pldt->iIsym != 3) {
            //		for(int j=0; j<pldt->iNg; j++) {
            //			x = pldt->dLcd[0][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
            //			y = pldt->dLcd[0][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
            //	    	one.append(QPointF(x,y));
            //   		}
            //   		
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			if(max < pldt->dLcd[0][j])
            //   				max = pldt->dLcd[0][j];
            //	  	}
            //   		
            //   	}
            //   	else {
            //   		// poszukac C180 w katach, ktory jest taki sam jak C0	 
            //   		b=0;  		
            //   		while(pldt->dC[b] < 90.0) {
            //    		b++;
            //   		}
            //   		for(a=0; a<pldt->iMc; a++) {
            //   			if(pldt->dC[a+b] == 180.0) {
            //				break;
            //  			}
            //  		}
            //		for(int j=0; j<pldt->iNg; j++) {
            //			x = pldt->dLcd[a][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
            //			y = pldt->dLcd[a][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
            //	    	one.append(QPointF(x,y));
            //   		}  		
            //   		
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			if(max < pldt->dLcd[a][j])
            //   				max = pldt->dLcd[a][j];
            //	  	}   		
            //
            //  	}
            if (elumdat.Isym != 3)
            {
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = elumdat.DLcd[0, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[0, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));

                    one.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[0, j])
                    {
                        max = elumdat.DLcd[0, j];
                    }
                }
            }
            else
            {
                b = 0;

                while (elumdat.DC[b] < 90)
                {
                    b++;
                }
                for (a = 0; a < elumdat.Mc; a++)
                {
                    if (elumdat.DC[a + b] == 180.0)
                    {
                        break;
                    }
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = elumdat.DLcd[a, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[a, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    one.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[a, j])
                    {
                        max = elumdat.DLcd[a, j];
                    }
                }
            }

            //   	// C180
            //   	if( (pldt->iIsym == 1) || (pldt->iIsym == 3) || (pldt->iIsym == 4) ) {
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			x = one[pldt->iNg-1-j].x();
            //   			y = one[pldt->iNg-1-j].y();
            //			one.append(QPointF(-x,y));
            //	  	}
            //  	}
            //  	else {
            //  		// poszukac C180 w katach	
            //   		for(a=0; a<pldt->iMc; a++) {
            //   			if(pldt->dC[a] == 180.0) {
            //				break;
            //  			}
            //  		}
            //		for(int j=pldt->iNg-1; j>=0; j--) {
            //			x = pldt->dLcd[a][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
            //			y = pldt->dLcd[a][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
            //	    	one.append(QPointF(-x,y));
            //   		}  		
            //   		
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			if(max < pldt->dLcd[a][j])
            //   				max = pldt->dLcd[a][j];
            //	  	}   		
            // 	}
            if (elumdat.Isym == 1 || elumdat.Isym == 3 || elumdat.Isym == 4)
            {
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = one[elumdat.Ng - 1 - j].X;
                    y = one[elumdat.Ng - 1 - j].Y;
                    one.Add(new Point(Convert.ToInt32(-x), Convert.ToInt32(y)));
                }
            }
            else
            {
                for (a = 0; a < elumdat.Mc; a++)
                {
                    if (elumdat.DC[a] == 180)
                    {
                        break;
                    }
                }
                for (var j = elumdat.Ng - 1; j >= 0; j--)
                {
                    x = elumdat.DLcd[a, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[a, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    one.Add(new Point(Convert.ToInt32(-x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[a, j])
                        max = elumdat.DLcd[a, j];
                }
            }





            //   	// C90
            //   	if(pldt->iIsym == 1) {
            //   		//two = one;
            //  	}
            //  	else if(pldt->iIsym == 3) {
            //		for(int j=0; j<pldt->iNg; j++) {
            //			x = pldt->dLcd[0][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
            //			y = pldt->dLcd[0][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
            //	    	two.append(QPointF(x,y));
            //   		}
            //   		
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			if(max < pldt->dLcd[0][j])
            //   				max = pldt->dLcd[0][j];
            //	  	}  		
            // 	}
            // 	else {
            //   		// poszukac C90
            //   		b=0;  		
            //   		for(a=0; a<pldt->iMc; a++) {
            //   			if(pldt->dC[a+b] == 90.0) {
            //				break;
            //  			}
            //  		}
            //		for(int j=0; j<pldt->iNg; j++) {
            //			x = pldt->dLcd[a][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
            //			y = pldt->dLcd[a][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
            //	    	two.append(QPointF(x,y));
            //   		}  		
            //   		
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			if(max < pldt->dLcd[a][j])
            //   				max = pldt->dLcd[a][j];
            //	  	} 		
            //	}

            if (elumdat.Isym == 1)
            {
                two = one;
            }
            else if (elumdat.Isym == 3)
            {
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = elumdat.DLcd[0, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[0, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    two.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[0, j])
                    {
                        max = elumdat.DLcd[0, j];
                    }
                }
            }
            else
            {
                b = 0;
                for (a = 0; a < elumdat.Mc; a++)
                {
                    if (elumdat.DC[a + b] == 90)
                    {
                        break;
                    }
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = elumdat.DLcd[a, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[a, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    two.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[a, j])
                    {
                        max = elumdat.DLcd[a, j];
                    }
                }
            }


            //    
            //    // C270
            //   	if(pldt->iIsym == 0) {

            if (elumdat.Isym == 0)
            {
                //   		b=0;  		
                //   		for(a=0; a<pldt->iMc; a++) {
                //   			if(pldt->dC[a+b] == 270.0) {
                //				break;
                //  			}
                //  		}
                b = 0;
                for (a = 0; a < elumdat.Mc; a++)
                {
                    if (elumdat.DC[a + b] == 270)
                    {
                        break;
                    }
                }

                //		for(int j=pldt->iNg-1; j>=0; j--) {
                //			x = pldt->dLcd[a][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
                //			y = pldt->dLcd[a][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
                //	    	two.append(QPointF(-x,y));
                //   		}  		
                //   		
                //   		for(int j=0; j<pldt->iNg; j++) {
                //   			if(max < pldt->dLcd[a][j])
                //   				max = pldt->dLcd[a][j];
                //	  	} 	   		
                //  	}
                for (var j = elumdat.Ng - 1; j >= 0; j--)
                {
                    x = elumdat.DLcd[a, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[a, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    two.Add(new Point(Convert.ToInt32(-x), Convert.ToInt32(y)));
                }
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    if (max < elumdat.DLcd[a, j])
                    {
                        max = elumdat.DLcd[a, j];
                    }
                }
            }






            //	if(pldt->iIsym == 3) {
            if (elumdat.Isym == 3)
            {
                //   		b=0;  	
                //   		while(pldt->dC[b] < 90.0) {
                //    		b++;
                //   		}   
                b = 0;
                while (elumdat.DC[b] < 90)
                {
                    b++;
                }
                //   		for(a=0; a<pldt->iMc; a++) {
                //   			if(pldt->dC[a+b] == 270.0) {
                //				break;
                //  			}
                //  		}
                for (a = 0; a < elumdat.Mc; a++)
                {
                    if (elumdat.DC[a + b] == 270)
                    {
                        break;
                    }
                }
                //		for(int j=pldt->iNg-1; j>=0; j--) {
                //			x = pldt->dLcd[a][j] * cos(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);
                //			y = pldt->dLcd[a][j] * sin(-(pldt->dG[j] * M_PI / 180.0) + M_PI_2);		
                //	    	two.append(QPointF(-x,y));
                //   		}  		
                //   		
                //   		for(int j=0; j<pldt->iNg; j++) {
                //   			if(max < pldt->dLcd[a][j])
                //   				max = pldt->dLcd[a][j];
                //	  	} 			
                for (var j = elumdat.Ng - 1; j >= 0; j--)
                {
                    x = elumdat.DLcd[a, j] * Math.Cos(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    y = elumdat.DLcd[a, j] * Math.Sin(-(elumdat.DG[j] * Math.PI / 180) + (Math.PI / 2));
                    two.Add(new Point(Convert.ToInt32(-x), Convert.ToInt32(y)));
                }

            }


            //  	}
            //   	if( (pldt->iIsym == 2) || (pldt->iIsym == 4) ) {
            //   		for(int j=0; j<pldt->iNg; j++) {
            //   			x = two[pldt->iNg-1-j].x();
            //   			y = two[pldt->iNg-1-j].y();
            //			two.append(QPointF(-x,y));
            //	  	}   		
            //  	}  	
            //    
            //}
            if (elumdat.Isym == 2 || elumdat.Isym == 4)
            {
                for (var j = 0; j < elumdat.Ng; j++)
                {
                    x = two[elumdat.Ng - 1 - j].X;
                    y = two[elumdat.Ng - 1 - j].Y;
                    two.Add(new Point(Convert.ToInt32(-x), Convert.ToInt32(y)));
                }
            }

            max = 20.0 * Math.Ceiling(max / 20.0);
            double marg = 60;
            double side = Math.Min(width - (int)marg, height - (int)marg);
            double scale = max / side * 2;


            var brush = Brushes.White;

            var bitmap = new Bitmap(width, height);
            var vector = Graphics.FromImage(bitmap);

            vector.TranslateTransform((width / 2), (height / 2));
            vector.FillRectangle(brush, new Rectangle(Convert.ToInt32(-side / 2 - marg / 2), Convert.ToInt32(-side / 2 - marg / 2), Convert.ToInt32(side + marg), Convert.ToInt32(side + marg)));

            var pen = new Pen(Color.Gray);
            vector.DrawEllipse(pen, new Rectangle(Convert.ToInt32(-side / 2), Convert.ToInt32(-side / 2), Convert.ToInt32(side), Convert.ToInt32(side)));
            vector.DrawEllipse(pen, new Rectangle(Convert.ToInt32(-side / 4), Convert.ToInt32(-side / 4), Convert.ToInt32(side / 2), Convert.ToInt32(side / 2)));
            vector.DrawEllipse(pen, new Rectangle(Convert.ToInt32(-side / 8), Convert.ToInt32(-side / 8), Convert.ToInt32(side / 4), Convert.ToInt32(side / 4)));
            vector.DrawEllipse(pen, new Rectangle(Convert.ToInt32(-side / 8 * 3), Convert.ToInt32(-side / 8 * 3), Convert.ToInt32(side / 4 * 3), Convert.ToInt32(side / 4 * 3)));

            for (int i = 0; i < 12; ++i)
            {
                vector.DrawLine(pen, 0, 0, Convert.ToInt32(side / 2), 0);
                vector.RotateTransform(30);
            }

            var newListOfPoints = new List<Point>();

            foreach (var point in two)
            {
                newListOfPoints.Add(new Point(Convert.ToInt32(-point.X / scale), Convert.ToInt32(-point.Y / scale)));
            }

            var path = new GraphicsPath();

            Point? firstPoint = null;

            foreach (var point in newListOfPoints)
            {
                if (firstPoint != null)
                {
                    path.AddLine(firstPoint.Value, point);
                }
                firstPoint = point;
            }

            var myBrush = new SolidBrush(Color.FromArgb(100, 255, 0, 0));

            vector.FillPath(myBrush, path);

            newListOfPoints = new List<Point>();

            foreach (var point in one)
            {
                newListOfPoints.Add(new Point(Convert.ToInt32(-point.X / scale), Convert.ToInt32(-point.Y / scale)));
            }

            path = new GraphicsPath();

            firstPoint = null;

            foreach (var point in newListOfPoints)
            {
                if (firstPoint != null)
                {
                    path.AddLine(firstPoint.Value, point);
                }
                firstPoint = point;
            }

            myBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 255));

            vector.FillPath(myBrush, path);

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            //
            //
            //    QRectF * test;
            //    test = new QRectF;
            //	int space = 2;    
            //    
            //    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            pen = new Pen(Color.Black);

            //    painter.setBrush(Qt::NoBrush);   
            var font = new Font("Arial", 8);
            float fl = (float)(side / 2);
            brush = Brushes.Black;
            vector.DrawString(max.ToString(), font, brush, 0, fl);
            //    painter.drawText(QRectF(QPointF(-100, side/2-marg), QSizeF(200, 2*marg)), Qt::AlignCenter,  QString("%1").arg(max), test);    


            //    painter.setPen(Qt::NoPen);
            //    painter.setBrush(QBrush(Qt::white, Qt::SolidPattern));
            ///////////////vector.FillRectangle();
            //    painter.drawRect(QRectF(QPointF(test->x()-space, test->y()), QSizeF(test->width()+2*space, test->height())));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, -(test->y())-test->height()), QSizeF(test->width()+2*space, test->height())));        
            //	painter.setPen(QPen(Qt::gray, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));      
            //    painter.setBrush(Qt::NoBrush);	
            //	painter.drawText(QRectF(QPointF(test->x()-test->width(), test->y()), QSizeF(3*test->width(), 2*test->height())), Qt::AlignCenter, QString("%1\n 0%2").arg(max).arg((char)176));   	

            //
            //	painter.drawText(QRectF(QPointF(test->x()-test->width(), -(test->y())-3*test->height()), QSizeF(3*test->width(), 3*test->height())), Qt::AlignHCenter | Qt::AlignBottom, QString(" 180%1\n%2").arg((char)176).arg(max));   		
            //	
            //  	
            //	painter.drawText(QRectF(QPointF(side/2, -test->height()/2), QSizeF(marg, test->height())), Qt::AlignVCenter | Qt::AlignLeft, QString(" 90%1").arg((char)176));
            //	painter.drawText(QRectF(QPointF(-side/2-marg, -test->height()/2), QSizeF(marg, test->height())), Qt::AlignVCenter | Qt::AlignRight, QString("90%1 ").arg((char)176));	
            //
            //	double x, y;
            //	x = side/2 * cos(-(30 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(30 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(x, y), QSizeF(marg, 2*test->height())), Qt::AlignTop | Qt::AlignLeft, QString(" 30%1").arg((char)176));	
            //	x = side/2 * cos(-(60 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(60 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(x, y), QSizeF(marg, 2*test->height())), Qt::AlignTop | Qt::AlignLeft, QString(" 60%1").arg((char)176));		
            //	x = side/2 * cos(-(120 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(120 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(x, y-2*test->height()), QSizeF(marg, 2*test->height())), Qt::AlignBottom | Qt::AlignLeft, QString(" 120%1").arg((char)176));	
            //	x = side/2 * cos(-(150 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(150 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(x, y-2*test->height()), QSizeF(marg, 2*test->height())), Qt::AlignBottom | Qt::AlignLeft, QString(" 150%1").arg((char)176));		
            //	x = side/2 * cos(-(30 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(30 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(-x-marg, y), QSizeF(marg, 2*test->height())), Qt::AlignTop | Qt::AlignRight, QString("30%1 ").arg((char)176));	
            //	x = side/2 * cos(-(60 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(60 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(-x-marg, y), QSizeF(marg, 2*test->height())), Qt::AlignTop | Qt::AlignRight, QString("60%1 ").arg((char)176));		
            //	x = side/2 * cos(-(120 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(120 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(-x-marg, y-2*test->height()), QSizeF(marg, 2*test->height())), Qt::AlignBottom | Qt::AlignRight, QString("120%1 ").arg((char)176));	
            //	x = side/2 * cos(-(150 * M_PI / 180.0) + M_PI_2);
            //	y = side/2 * sin(-(150 * M_PI / 180.0) + M_PI_2);		
            //	painter.drawText(QRectF(QPointF(-x-marg, y-2*test->height()), QSizeF(marg, 2*test->height())), Qt::AlignBottom | Qt::AlignRight, QString("150%1 ").arg((char)176));
            //	
            //    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //    painter.setBrush(Qt::NoBrush);    
            //    painter.drawText(QRectF(QPointF(-100, side/4-marg), QSizeF(200, 2*marg)), Qt::AlignCenter,  QString("%1").arg(max/2), test);       
            //    painter.setPen(Qt::NoPen);
            //    painter.setBrush(QBrush(Qt::white, Qt::SolidPattern));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, test->y()), QSizeF(test->width()+2*space, test->height())));    
            //    painter.drawRect(QRectF(QPointF(test->x()-space, -(test->y())-test->height()), QSizeF(test->width()+2*space, test->height())));    
            //	painter.setPen(QPen(Qt::gray, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));      
            //    painter.setBrush(Qt::NoBrush);	
            //	painter.drawText(*test, Qt::AlignCenter,  QString("%1").arg(max/2));
            //	painter.drawText(QRectF(QPointF(test->x(), -(test->y())-test->height()), QSizeF(test->width(), test->height())), Qt::AlignCenter,  QString("%1").arg(max/2));	
            //
            //    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //    painter.setBrush(Qt::NoBrush);    
            //    painter.drawText(QRectF(QPointF(-100, side/8-marg), QSizeF(200, 2*marg)), Qt::AlignCenter,  QString("%1").arg(max/4), test);       
            //    painter.setPen(Qt::NoPen);
            //    painter.setBrush(QBrush(Qt::white, Qt::SolidPattern));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, test->y()), QSizeF(test->width()+2*space, test->height())));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, -(test->y())-test->height()), QSizeF(test->width()+2*space, test->height())));        
            //	painter.setPen(QPen(Qt::gray, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));      
            //    painter.setBrush(Qt::NoBrush);	
            //	painter.drawText(*test, Qt::AlignCenter,  QString("%1").arg(max/4));
            //	painter.drawText(QRectF(QPointF(test->x(), -(test->y())-test->height()), QSizeF(test->width(), test->height())), Qt::AlignCenter,  QString("%1").arg(max/4));		
            //
            //	painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //    painter.setBrush(Qt::NoBrush);    
            //    painter.drawText(QRectF(QPointF(-100, side/8*3-marg), QSizeF(200, 2*marg)), Qt::AlignCenter,  QString("%1").arg(max/4*3), test);       
            //    painter.setPen(Qt::NoPen);
            //    painter.setBrush(QBrush(Qt::white, Qt::SolidPattern));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, test->y()), QSizeF(test->width()+2*space, test->height())));
            //    painter.drawRect(QRectF(QPointF(test->x()-space, -(test->y())-test->height()), QSizeF(test->width()+2*space, test->height())));    
            //	painter.setPen(QPen(Qt::gray, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));      
            //    painter.setBrush(Qt::NoBrush);	
            //	painter.drawText(*test, Qt::AlignCenter,  QString("%1").arg(max/4*3));
            //	painter.drawText(QRectF(QPointF(test->x(), -(test->y())-test->height()), QSizeF(test->width(), test->height())), Qt::AlignCenter,  QString("%1").arg(max/4*3));		
            //   
            //    
            //	int alpha = 48;
            //	
            //	// C90-C270	
            //	
            //	QVector<QPointF> pointsC90C270;     


            //    for(int i=0; i<two.size(); i++) {
            //    	pointsC90C270.append(QPointF(-two[i].x() / scale, two[i].y() / scale));   	
            //   	}
            //    
            //    QPainterPath pathC90C270;   
            //    pathC90C270.moveTo(pointsC90C270[0]);
            //    
            //    for(int i=1; i<pointsC90C270.size(); i++) {
            //    	pathC90C270.lineTo(pointsC90C270[i]);
            //   	}
            //   
            //    painter.setPen(QPen(Qt::red, 2, Qt::DashLine, Qt::RoundCap, Qt::RoundJoin));   
            //    painter.setBrush(QBrush(QColor(255, 0, 0, alpha), Qt::SolidPattern));
            //    if(Vars().fill) {
            //	    painter.setBrush(QBrush(QColor(255, 0, 0, alpha), Qt::SolidPattern));    	
            //   	}
            //   	else {
            //   		painter.setBrush(Qt::NoBrush);
            //  	}    
            //    painter.drawPath(pathC90C270);



            //
            //    // C0-C180
            //    
            //    QVector<QPointF> pointsC0C180;       
            //    for(int i=0; i<one.size(); i++) {
            //    	pointsC0C180.append(QPointF(-one[i].x() / scale, one[i].y() / scale));   	
            //   	}
            //    
            //    QPainterPath pathC0C180;   
            //    pathC0C180.moveTo(pointsC0C180[0]);
            //    
            //    for(int i=1; i<pointsC0C180.size(); i++) {
            //    	pathC0C180.lineTo(pointsC0C180[i]);
            //   	}
            //   
            //    painter.setPen(QPen(Qt::blue, 2, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin)); 
            //    if(Vars().fill) {
            //	    painter.setBrush(QBrush(QColor(0, 0, 255, alpha), Qt::SolidPattern));      	
            //   	}
            //   	else {
            //   		painter.setBrush(Qt::NoBrush);
            //  	}
            //  
            //    painter.drawPath(pathC0C180);
            //    
            //	if(Vars().legend == true) {  
            //	    // legend
            //	    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //	    painter.setBrush(Qt::NoBrush);    
            //	    painter.drawText(QRectF(QPointF(side/2-100, side/2-test->height()), QSizeF(100, 2*test->height())), Qt::AlignRight | Qt::AlignBottom,  "cd/1000lm"); 
            //	    
            //	    if(Vars().mainWindow->ldt->iIsym == 1) {
            //	    	painter.setPen(QPen(Qt::blue, 2, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin));
            //		    painter.setBrush(QBrush(QColor(0, 0, 255, alpha), Qt::SolidPattern));
            //		    painter.drawRect(QRectF(QPointF(-side/2-test->height(), side/2), QSizeF(test->height(), test->height())));    
            //		        
            //		    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //		    painter.setBrush(Qt::NoBrush);    
            //		    painter.drawText(QRectF(QPointF(-side/2+0.5*test->height(), side/2), QSizeF(side, 2*test->height())), Qt::AlignLeft,  "C0-C180 / C90-C270");
            //	     	
            //	   	}
            //		else {
            //		    painter.setPen(QPen(Qt::blue, 2, Qt::SolidLine, Qt::RoundCap, Qt::RoundJoin)); 
            //		    painter.setBrush(QBrush(QColor(0, 0, 255, alpha), Qt::SolidPattern));
            //		    painter.drawRect(QRectF(QPointF(-side/2-test->height(), side/2-1.5*test->height()), QSizeF(test->height(), test->height())));    
            //		        
            //		    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //		    painter.setBrush(Qt::NoBrush);    
            //		    painter.drawText(QRectF(QPointF(-side/2+0.5*test->height(), side/2-1.5*test->height()), QSizeF(side, 2*test->height())), Qt::AlignLeft,  "C0-C180");    
            //		    
            //		    painter.setPen(QPen(Qt::red, 2, Qt::DashLine, Qt::RoundCap, Qt::RoundJoin)); 
            //		    painter.setBrush(QBrush(QColor(255, 0, 0, alpha), Qt::SolidPattern));
            //		    painter.drawRect(QRectF(QPointF(-side/2-test->height(), side/2), QSizeF(test->height(), test->height())));    
            //		        
            //		    painter.setPen(QPen(Qt::black, 0, Qt::SolidLine, Qt::FlatCap, Qt::RoundJoin));
            //		    painter.setBrush(Qt::NoBrush);    
            //		    painter.drawText(QRectF(QPointF(-side/2+0.5*test->height(), side/2), QSizeF(side, 2*test->height())), Qt::AlignLeft,  "C90-C270");
            //	    		
            //		}
            //	}
            //
            //}

            vector.Save();
            bitmap.Save(@"C:\temp\qwertyuiop1234567890.bmp");
        }



        private static General SetupGeneral(Elumdat elumdat)
        {
            var identification = elumdat.SIden;

            var typeIndicator =
                elumdat.Ityp == 1 ?
                TypeIndicator.WithPointSource :
                TypeIndicator.LinearLuminaire;

            SymmetryIndicator symmetryIndicator;

            switch (elumdat.Isym)
            {
                case 1:
                    symmetryIndicator = SymmetryIndicator.VerticalAxis;
                    break;
                case 2:
                    symmetryIndicator = SymmetryIndicator.C0ToC180;
                    break;
                case 3:
                    symmetryIndicator = SymmetryIndicator.C90ToC270;
                    break;
                case 4:
                    symmetryIndicator = SymmetryIndicator.C0ToC180AndC90ToC270;
                    break;
                default:
                    symmetryIndicator = SymmetryIndicator.None;
                    break;
            }

            var numberOfCplanes = elumdat.Nc;
            var distanceBetween = Convert.ToDecimal(elumdat.DDc);
            var luminousIntensitiesInEachCPlane = elumdat.Ng;
            var distanceBetweenLuminousIntensitiesPerCPlane = Convert.ToDecimal(elumdat.DDg);
            var measurementReportNumber = elumdat.SMrn;
            var luminaireName = elumdat.SLnam;
            var luminaireNumber = elumdat.SLnum;
            var fileName = elumdat.SFnam;
            var dateUser = elumdat.SDate;


            return new General(identification, typeIndicator, symmetryIndicator, numberOfCplanes, distanceBetween, luminousIntensitiesInEachCPlane, distanceBetweenLuminousIntensitiesPerCPlane, measurementReportNumber, luminaireName, luminaireNumber, fileName, dateUser);
        }

        private static Luminaire SetupLuminaire(Elumdat elumdat)
        {
            var length = Convert.ToDecimal(elumdat.Dl);
            var width = Convert.ToDecimal(elumdat.Db);
            var height = Convert.ToDecimal(elumdat.Dh);
            var areaLength = Convert.ToDecimal(elumdat.DLa);
            var areaWidth = Convert.ToDecimal(elumdat.Db1);
            var c0PlaneHeight = Convert.ToDecimal(elumdat.DHc0);
            var c90PlaneHeight = Convert.ToDecimal(elumdat.DHc90);
            var c180PlaneHeight = Convert.ToDecimal(elumdat.DHc180);
            var c270PlaneHeight = Convert.ToDecimal(elumdat.DHc270);
            var downwardFlux = Convert.ToDecimal(elumdat.DDff);
            var lightOutput = Convert.ToDecimal(elumdat.DLorl);
            var intensityConversionFactor = Convert.ToDecimal(elumdat.DCfli);
            var tilt = Convert.ToDecimal(elumdat.DTilt);


            return new Luminaire(length, width, height, areaLength, areaWidth, c0PlaneHeight, c90PlaneHeight, c180PlaneHeight, c270PlaneHeight, downwardFlux, lightOutput, intensityConversionFactor, tilt);
        }

        private static IEnumerable<LampSet> SetupLamps(Elumdat elumdat)
        {
            return (from lamp in elumdat.Lamps
                    let count = lamp.Nl
                    let type = lamp.STl
                    let luminousFlux = Convert.ToDecimal(lamp.DTlf)
                    let colorTemperature = lamp.SCa
                    let colorRenderingIndex = lamp.SCrg
                    let wattage = Convert.ToDecimal(lamp.DWb)
                    select new LampSet(count, type, luminousFlux, colorTemperature, colorRenderingIndex, wattage))
                    .ToList();
        }

        private static UtilizationFactors SetupUtilizationFactors(Elumdat elumdat)
        {
            var k060 = Convert.ToDecimal(elumdat.DDr[0]);
            var k080 = Convert.ToDecimal(elumdat.DDr[1]);
            var k1 = Convert.ToDecimal(elumdat.DDr[2]);
            var k125 = Convert.ToDecimal(elumdat.DDr[3]);
            var k15 = Convert.ToDecimal(elumdat.DDr[4]);
            var k2 = Convert.ToDecimal(elumdat.DDr[5]);
            var k25 = Convert.ToDecimal(elumdat.DDr[6]);
            var k3 = Convert.ToDecimal(elumdat.DDr[7]);
            var k4 = Convert.ToDecimal(elumdat.DDr[8]);
            var k5 = Convert.ToDecimal(elumdat.DDr[9]);

            return new UtilizationFactors(k060, k080, k1, k125, k15, k2, k25, k3, k4, k5);
        }
    }
}
