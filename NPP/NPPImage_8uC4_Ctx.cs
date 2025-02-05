//	Copyright (c) 2012, Michael Kunz. All rights reserved.
//	http://kunzmi.github.io/managedCuda
//
//	This file is part of ManagedCuda.
//
//	ManagedCuda is free software: you can redistribute it and/or modify
//	it under the terms of the GNU Lesser General Public License as 
//	published by the Free Software Foundation, either version 2.1 of the 
//	License, or (at your option) any later version.
//
//	ManagedCuda is distributed in the hope that it will be useful,
//	but WITHOUT ANY WARRANTY; without even the implied warranty of
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//	GNU Lesser General Public License for more details.
//
//	You should have received a copy of the GNU Lesser General Public
//	License along with this library; if not, write to the Free Software
//	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
//	MA 02110-1301  USA, http://www.gnu.org/licenses/.


#define ADD_MISSING_CTX
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ManagedCuda.BasicTypes;

namespace ManagedCuda.NPP
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NPPImage_8uC4 : NPPImageBase
    {
        #region Copy
        /// <summary>
        /// Image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="channel">Channel number. This number is added to the dst pointer</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC1 dst, int channel, NppStreamContext nppStreamCtx)
        {
            if (channel < 0 | channel >= _channels) throw new ArgumentOutOfRangeException("channel", "channel must be in range [0..3].");
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_C4C1R_Ctx(_devPtrRoi + channel * _typeSize, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_C4C1R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="channelSrc">Channel number. This number is added to the src pointer</param>
        /// <param name="channelDst">Channel number. This number is added to the dst pointer</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC4 dst, int channelSrc, int channelDst, NppStreamContext nppStreamCtx)
        {
            if (channelSrc < 0 | channelSrc >= _channels) throw new ArgumentOutOfRangeException("channelSrc", "channelSrc must be in range [0..2].");
            if (channelDst < 0 | channelDst >= dst.Channels) throw new ArgumentOutOfRangeException("channelDst", "channelDst must be in range [0..2].");
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_C4CR_Ctx(_devPtrRoi + channelSrc * _typeSize, _pitch, dst.DevicePointerRoi + channelDst * _typeSize, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_C4CR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="mask">Mask image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC4 dst, NPPImage_8uC1 mask, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_C4MR_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_C4MR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="mask">Mask image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CopyA(NPPImage_8uC4 dst, NPPImage_8uC1 mask, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_AC4MR_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_AC4MR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Masked Operation 8-bit unsigned image copy. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CopyA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Copy image and pad borders with a constant, user-specifiable color.
        /// </summary>
        /// <param name="dst">Destination image. The image ROI defines the destination region, i.e. the region that gets filled with data from
        /// the source image (inner part) and constant border color (outer part).</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        /// <param name="nValue">The pixel value to be set for border pixels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyConstBorder.nppiCopyConstBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyConstBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Copy image and pad borders with a constant, user-specifiable color. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image. The image ROI defines the destination region, i.e. the region that gets filled with data from
        /// the source image (inner part) and constant border color (outer part).</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        /// <param name="nValue">The pixel value to be set for border pixels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CopyA(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyConstBorder.nppiCopyConstBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyConstBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Three-channel 8-bit unsigned packed to planar image copy.
        /// </summary>
        /// <param name="dst0">Destination image channel 0</param>
        /// <param name="dst1">Destination image channel 1</param>
        /// <param name="dst2">Destination image channel 2</param>
        /// <param name="dst3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Copy(NPPImage_8uC1 dst0, NPPImage_8uC1 dst1, NPPImage_8uC1 dst2, NPPImage_8uC1 dst3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { dst0.DevicePointerRoi, dst1.DevicePointerRoi, dst2.DevicePointerRoi, dst3.DevicePointerRoi };
            status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_C4P4R_Ctx(_devPtrRoi, _pitch, array, dst0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_C4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Three-channel 8-bit unsigned planar to packed image copy.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void Copy(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.MemCopy.nppiCopy_8u_P4C4R_Ctx(array, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopy_8u_P4C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Set
        /// <summary>
        /// Set pixel values to nValue.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Set(byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemSet.nppiSet_8u_C4R_Ctx(nValue, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 4)</param>
        /// <param name="mask">Mask image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Set(byte[] nValue, NPPImage_8uC1 mask, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemSet.nppiSet_8u_C4MR_Ctx(nValue, _devPtrRoi, _pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_8u_C4MR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value.
        /// </summary>
        /// <param name="nValue">Value to be set</param>
        /// <param name="channel">Channel number. This number is added to the dst pointer</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Set(byte nValue, int channel, NppStreamContext nppStreamCtx)
        {
            if (channel < 0 | channel >= _channels) throw new ArgumentOutOfRangeException("channel", "channel must be in range [0..3].");
            status = NPPNativeMethods_Ctx.NPPi.MemSet.nppiSet_8u_C4CR_Ctx(nValue, _devPtrRoi + channel * _typeSize, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_8u_C4CR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value. Not affecting alpha channel.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 3)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SetA(byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemSet.nppiSet_8u_AC4R_Ctx(nValue, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Set pixel values to nValue. <para/>
        /// The 8-bit mask image affects setting of the respective pixels in the destination image. <para/>
        /// If the mask value is zero (0) the pixel is not set, if the mask is non-zero, the corresponding
        /// destination pixel is set to specified value. Not affecting alpha channel.
        /// </summary>
        /// <param name="nValue">Value to be set (Array size = 3)</param>
        /// <param name="mask">Mask image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SetA(byte[] nValue, NPPImage_8uC1 mask, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MemSet.nppiSet_8u_AC4MR_Ctx(nValue, _devPtrRoi, _pitch, _sizeRoi, mask.DevicePointerRoi, mask.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSet_8u_AC4MR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Add
        /// <summary>
        /// Image addition, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Add(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Add.nppiAdd_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Add(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Add.nppiAdd_8u_C4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Add(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AddConst.nppiAddC_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Add(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AddConst.nppiAddC_8u_C4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image addition, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AddA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Add.nppiAdd_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image addition, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AddA(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Add.nppiAdd_8u_AC4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAdd_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AddA(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AddConst.nppiAddC_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Add constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unmodified Alpha.
        /// </summary>
        /// <param name="nConstant">Values to add</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AddA(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AddConst.nppiAddC_8u_AC4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAddC_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Logical
        /// <summary>
        /// image bit shift by constant (left).
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LShiftC(uint[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LeftShiftConst.nppiLShiftC_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left), inplace.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LShiftC(uint[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LeftShiftConst.nppiLShiftC_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left). Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LShiftCA(uint[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LeftShiftConst.nppiLShiftC_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (left), inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LShiftCA(uint[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LeftShiftConst.nppiLShiftC_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLShiftC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image bit shift by constant (right).
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RShiftC(uint[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RightShiftConst.nppiRShiftC_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right), inplace.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RShiftC(uint[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RightShiftConst.nppiRShiftC_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right). Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RShiftCA(uint[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RightShiftConst.nppiRShiftC_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image bit shift by constant (right), inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Constant (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RShiftCA(uint[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RightShiftConst.nppiRShiftC_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRShiftC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void And(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.And.nppiAnd_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void And(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.And.nppiAnd_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void And(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AndConst.nppiAndC_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void And(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AndConst.nppiAndC_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AndA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.And.nppiAnd_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AndA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.And.nppiAnd_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAnd_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical and with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AndA(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AndConst.nppiAndC_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical and with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AndA(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AndConst.nppiAndC_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAndC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Or(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Or.nppiOr_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Or(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Or.nppiOr_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Or(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.OrConst.nppiOrC_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Or(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.OrConst.nppiOrC_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void OrA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Or.nppiOr_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void OrA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Or.nppiOr_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOr_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Or with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void OrA(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.OrConst.nppiOrC_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Or with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void OrA(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.OrConst.nppiOrC_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiOrC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Xor(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Xor.nppiXor_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Xor(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Xor.nppiXor_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Xor(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.XorConst.nppiXorC_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor with constant.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Xor(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.XorConst.nppiXorC_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void XorA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Xor.nppiXor_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void XorA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Xor.nppiXor_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXor_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image logical Xor with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void XorA(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.XorConst.nppiXorC_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Xor with constant. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value (Array length = 4)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void XorA(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.XorConst.nppiXorC_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXorC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Image logical Not.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Not(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Not.nppiNot_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNot_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Not.
        /// </summary>
        public void Not(NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Not.nppiNot_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNot_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Image logical Not. Unchanged Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NotA(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Not.nppiNot_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNot_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image logical Not. Unchanged Alpha.
        /// </summary>
        public void NotA(NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Not.nppiNot_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNot_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sub
        /// <summary>
        /// Image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sub(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sub.nppiSub_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sub(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sub.nppiSub_8u_C4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sub(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SubConst.nppiSubC_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sub(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SubConst.nppiSubC_8u_C4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SubA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sub.nppiSub_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image subtraction, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SubA(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sub.nppiSub_8u_AC4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSub_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SubA(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SubConst.nppiSubC_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Subtract constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value to subtract</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SubA(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SubConst.nppiSubC_8u_AC4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSubC_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Mul
        /// <summary>
        /// Image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Mul.nppiMul_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Mul.nppiMul_8u_C4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConst.nppiMulC_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConst.nppiMulC_8u_C4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication and scale by max bit width value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulScale.nppiMulScale_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication and scale by max bit width value
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulScale.nppiMulScale_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image and scale by max bit width value
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConstScale.nppiMulCScale_8u_C4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image and scale by max bit width value
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mul(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConstScale.nppiMulCScale_8u_C4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Mul.nppiMul_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Mul.nppiMul_8u_AC4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMul_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConst.nppiMulC_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConst.nppiMulC_8u_AC4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulC_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image multiplication and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulScale.nppiMulScale_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image multiplication and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulScale.nppiMulScale_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulScale_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Multiply constant to image and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(byte[] nConstant, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConstScale.nppiMulCScale_8u_AC4R_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Multiply constant to image and scale by max bit width value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MulA(byte[] nConstant, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MulConstScale.nppiMulCScale_8u_AC4IR_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMulCScale_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Div
        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Div.nppiDiv_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Div.nppiDiv_8u_C4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivConst.nppiDivC_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivConst.nppiDivC_8u_C4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppRoundMode rndMode, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivRound.nppiDiv_Round_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, rndMode, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Div(NPPImage_8uC4 src2, NppRoundMode rndMode, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivRound.nppiDiv_Round_8u_C4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, rndMode, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Div.nppiDiv_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(NPPImage_8uC4 src2, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Div.nppiDiv_8u_AC4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(byte[] nConstant, NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivConst.nppiDivC_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, nConstant, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Divide constant to image, scale by 2^(-nScaleFactor), then clamp to saturated value. Inplace. Unchanged Alpha.
        /// </summary>
        /// <param name="nConstant">Value</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(byte[] nConstant, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivConst.nppiDivC_8u_AC4IRSfs_Ctx(nConstant, _devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDivC_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppRoundMode rndMode, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivRound.nppiDiv_Round_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, rndMode, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image division, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="rndMode">Result Rounding mode to be used</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DivA(NPPImage_8uC4 src2, NppRoundMode rndMode, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.DivRound.nppiDiv_Round_8u_AC4IRSfs_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, rndMode, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDiv_Round_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sqr
        /// <summary>
        /// Image squared, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sqr(NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqr.nppiSqr_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image squared, scale by 2^(-nScaleFactor), then clamp to saturated value.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sqr(int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqr.nppiSqr_8u_C4IRSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_8u_C4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image squared, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrA(NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqr.nppiSqr_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image squared, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrA(int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqr.nppiSqr_8u_AC4IRSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqr_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Sqrt
        ///// <summary>
        ///// Image square root, scale by 2^(-nScaleFactor), then clamp to saturated value.
        ///// </summary>
        ///// <param name="dest">Destination image</param>
        ///// <param name="nScaleFactor">scaling factor</param>
        //public void Sqrt(NPPImage_8uC4 dest, int nScaleFactor)
        //{
        //	status = NPPNativeMethods_Ctx.NPPi.Sqrt.nppiSqrt_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_8u_C4RSfs_Ctx", status));
        //	NPPException.CheckNppStatus(status, this);
        //}

        ///// <summary>
        ///// Inplace image square root, scale by 2^(-nScaleFactor), then clamp to saturated value.
        ///// </summary>
        ///// <param name="nScaleFactor">scaling factor</param>
        //public void Sqrt(int nScaleFactor)
        //{
        //	status = NPPNativeMethods_Ctx.NPPi.Sqrt.nppiSqrt_8u_C4IRSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_8u_C4IRSfs_Ctx", status));
        //	NPPException.CheckNppStatus(status, this);
        //}

        /// <summary>
        /// Image square root, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrtA(NPPImage_8uC4 dest, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqrt.nppiSqrt_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image square root, scale by 2^(-nScaleFactor), then clamp to saturated value. Unchanged Alpha.
        /// </summary>
        /// <param name="nScaleFactor">scaling factor</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrtA(int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Sqrt.nppiSqrt_8u_AC4IRSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrt_8u_AC4IRSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Morphology
        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Dilate(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiDilate_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Erode(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiErode_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Histogram
        /// <summary>
        /// Scratch-buffer size for HistogramEven.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// <returns></returns>
        public int HistogramEvenGetBufferSize(int[] nLevels, NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEvenGetBufferSize_8u_C4R_Ctx(_sizeRoi, nLevels, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEvenGetBufferSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Compute levels with even distribution.
        /// </summary>
        /// <param name="nLevels">The number of levels being computed. nLevels must be at least 2, otherwise an NPP_-
        /// HISTO_NUMBER_OF_LEVELS_ERROR error is returned.</param>
        /// <param name="nLowerBound">Lower boundary value of the lowest level.</param>
        /// <param name="nUpperBound">Upper boundary value of the greatest level.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// <returns>An array of size nLevels which receives the levels being computed.</returns>
        public int[] EvenLevels(int nLevels, int nLowerBound, int nUpperBound, NppStreamContext nppStreamCtx)
        {
            int[] Levels = new int[nLevels];
            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiEvenLevelsHost_32s_Ctx(Levels, nLevels, nLowerBound, nUpperBound, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiEvenLevelsHost_32s_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return Levels;
        }

        /// <summary>
        /// Scratch-buffer size for HistogramEven. Not affecting Alpha channel. 
        /// </summary>
        /// <param name="nLevels"></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// <returns></returns>
        public int HistogramEvenGetBufferSizeA(int[] nLevels, NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEvenGetBufferSize_8u_AC4R_Ctx(_sizeRoi, nLevels, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEvenGetBufferSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Histogram with evenly distributed bins. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (4 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 4</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 4</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramEven(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1, histogram[3].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };


            int bufferSize = HistogramEvenGetBufferSize(size, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEven_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. No additional buffer is allocated.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (4 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 4</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 4</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramEvenGetBufferSize(int[])"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramEven(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1, histogram[3].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };

            int bufferSize = HistogramEvenGetBufferSize(size, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEven_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. Buffer is internally allocated and freed. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (3 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 3</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramEvenA(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };


            int bufferSize = HistogramEvenGetBufferSizeA(size, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEven_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with evenly distributed bins. No additional buffer is allocated. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">Allocated device memory of size nLevels (3 Variables)</param>
        /// <param name="nLowerLevel">Lower boundary of lowest level bin. E.g. 0 for [0..255]. Size = 3</param>
        /// <param name="nUpperLevel">Upper boundary of highest level bin. E.g. 256 for [0..255]. Size = 3</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramEvenGetBufferSizeA(int[])"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramEvenA(CudaDeviceVariable<int>[] histogram, int[] nLowerLevel, int[] nUpperLevel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size + 1, histogram[1].Size + 1, histogram[2].Size + 1 };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };

            int bufferSize = HistogramEvenGetBufferSizeA(size, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramEven_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, size, nLowerLevel, nUpperLevel, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramEven_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Scratch-buffer size for HistogramRange.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// <returns></returns>
        public int HistogramRangeGetBufferSize(int[] nLevels, NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRangeGetBufferSize_8u_C4R_Ctx(_sizeRoi, nLevels, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRangeGetBufferSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Scratch-buffer size for HistogramRange. Not affecting Alpha channel.
        /// </summary>
        /// <param name="nLevels"></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// <returns></returns>
        public int HistogramRangeGetBufferSizeA(int[] nLevels, NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRangeGetBufferSize_8u_AC4R_Ctx(_sizeRoi, nLevels, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRangeGetBufferSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 4</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 4</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramRange(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size, histogram[3].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSize(size, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRange_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. No additional buffer is allocated.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 4</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 4</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramRangeGetBufferSize(int[])"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramRange(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size, histogram[3].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer, histogram[3].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSize(size, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRange_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. Buffer is internally allocated and freed. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 3</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramRangeA(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSizeA(size, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRange_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Histogram with bins determined by pLevels array. No additional buffer is allocated. Alpha channel is ignored during the histograms computations.
        /// </summary>
        /// <param name="histogram">array that receives the computed histogram. The CudaDeviceVariable must be of size nLevels-1. Array size = 3</param>
        /// <param name="pLevels">Array in device memory containing the level sizes of the bins. The CudaDeviceVariable must be of size nLevels. Array size = 3</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="HistogramRangeGetBufferSizeA(int[])"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HistogramRangeA(CudaDeviceVariable<int>[] histogram, CudaDeviceVariable<int>[] pLevels, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int[] size = new int[] { histogram[0].Size, histogram[1].Size, histogram[2].Size };
            CUdeviceptr[] devPtrs = new CUdeviceptr[] { histogram[0].DevicePointer, histogram[1].DevicePointer, histogram[2].DevicePointer };
            CUdeviceptr[] devLevels = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };

            int bufferSize = HistogramRangeGetBufferSizeA(size, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Histogram.nppiHistogramRange_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, devPtrs, devLevels, size, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHistogramRange_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMax
        /// <summary>
        /// Scratch-buffer size for MinMax.
        /// </summary>
        /// <returns></returns>
        public int MinMaxGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MinMaxNew.nppiMinMaxGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMaxGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum and maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinMax(CudaDeviceVariable<byte> min, CudaDeviceVariable<byte> max, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinMaxGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MinMaxNew.nppiMinMax_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum and maximum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinMaxGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinMax(CudaDeviceVariable<byte> min, CudaDeviceVariable<byte> max, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinMaxGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MinMaxNew.nppiMinMax_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, min.DevicePointer, max.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinMax_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter
        /// <summary>
        /// Pixels under the mask are multiplied by the respective weights in the mask and the results are summed.<para/>
        /// Before writing the result pixel the sum is scaled back via division by nDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="aKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Filter(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, NppiSize aKernelSize, NppiPoint oAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, aKernelSize, oAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply convolution filter with user specified 1D column of weights. Result pixel is equal to the sum of
        /// the products between the kernel coefficients (pKernel array) and corresponding neighboring column pixel
        /// values in the source image defined by nKernelDim and nAnchorY, divided by nDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumn(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumn_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRow(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRow_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel with border control. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowBorder(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRowBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBox(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFixedFilters2D.nppiFilterBox_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBox_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMin(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMin_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMin_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMax(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMax_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMax_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ColorTwist
        /// <summary>
        /// An input color twist matrix with floating-point pixel values is applied
        /// within ROI. Alpha channel is the last channel and is not processed.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="twistMatrix">The color twist matrix with floating-point pixel values [3,4].</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwistA(NPPImage_8uC4 dest, float[,] twistMatrix, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorProcessing.nppiColorTwist32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, twistMatrix, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned color twist, with alpha copy.<para/>
        /// An input color twist matrix with floating-point coefficient values is applied with in ROI.<para/>
        /// Alpha channel is the last channel and is copied unmodified from the source pixel to the destination pixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="twistMatrix">The color twist matrix with floating-point pixel values [3,4].</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwist(NPPImage_8uC4 dest, float[,] twistMatrix, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorTwist.nppiColorTwist32f_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, twistMatrix, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned in place color twist, not affecting Alpha.<para/>
        /// An input color twist matrix with floating-point coefficient values is applied with in ROI.<para/>
        /// Alpha channel is the last channel and is unmodified.
        /// </summary>
        /// <param name="twistMatrix">The color twist matrix with floating-point pixel values [3,4].</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwist(float[,] twistMatrix, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorTwist.nppiColorTwist32f_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, twistMatrix, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned color twist with 4x4 matrix and constant vector addition.
        /// An input 4x4 color twist matrix with floating-point coefficient values with an additional constant vector addition
        /// is applied within ROI.  For this particular version of the function the result is generated as shown below.
        ///  \code
        ///      dst[0] = aTwist[0][0]		/// src[0] + aTwist[0][1]		/// src[1] + aTwist[0][2]		/// src[2] + aTwist[0][3]		/// src[3] + aConstants[0]
        ///      dst[1] = aTwist[1][0]		/// src[0] + aTwist[1][1]		/// src[1] + aTwist[1][2]		/// src[2] + aTwist[1][3]		/// src[3] + aConstants[1]
        ///      dst[2] = aTwist[2][0]		/// src[0] + aTwist[2][1]		/// src[1] + aTwist[2][2]		/// src[2] + aTwist[2][3]		/// src[3] + aConstants[2]
        ///      dst[3] = aTwist[3][0]		/// src[0] + aTwist[3][1]		/// src[1] + aTwist[3][2]		/// src[2] + aTwist[3][3]		/// src[3] + aConstants[3]
        ///  \endcode
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="twistMatrix">The color twist matrix with floating-point coefficient values [4,4].</param>
        /// <param name="aConstants">fixed size array of constant values, one per channel [4]</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwistC(NPPImage_8uC4 dest, float[,] twistMatrix, float[] aConstants, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorTwist.nppiColorTwist32fC_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, twistMatrix, aConstants, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32fC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned in place color twist with 4x4 matrix and an additional constant vector addition.
        /// An input 4x4 color twist matrix with floating-point coefficient values with an additional constant vector addition
        /// is applied within ROI.  For this particular version of the function the result is generated as shown below.
        ///  \code
        ///      dst[0] = aTwist[0][0]		/// src[0] + aTwist[0][1]		/// src[1] + aTwist[0][2]		/// src[2] + aTwist[0][3]		/// src[3] + aConstants[0]
        ///      dst[1] = aTwist[1][0]		/// src[0] + aTwist[1][1]		/// src[1] + aTwist[1][2]		/// src[2] + aTwist[1][3]		/// src[3] + aConstants[1]
        ///      dst[2] = aTwist[2][0]		/// src[0] + aTwist[2][1]		/// src[1] + aTwist[2][2]		/// src[2] + aTwist[2][3]		/// src[3] + aConstants[2]
        ///      dst[3] = aTwist[3][0]		/// src[0] + aTwist[3][1]		/// src[1] + aTwist[3][2]		/// src[2] + aTwist[3][3]		/// src[3] + aConstants[3]
        ///  \endcode
        /// </summary>
        /// <param name="twistMatrix">The color twist matrix with floating-point coefficient values [4,4].</param>
        /// <param name="aConstants">fixed size array of constant values, one per channel [4]</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwistC(float[,] twistMatrix, float[] aConstants, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorTwist.nppiColorTwist32fC_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, twistMatrix, aConstants, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32fC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion
        #region Geometric Transforms


        /// <summary>
        /// Resizes images.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Resize(NPPImage_8uC4 dest, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResize_8u_C4R_Ctx(_devPtr, _pitch, _sizeOriginal, new NppiRect(_pointRoi, _sizeRoi), dest.DevicePointer, dest.Pitch, dest.Size, new NppiRect(dest.PointRoi, dest.SizeRoi), eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Resizes images. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ResizeA(NPPImage_8uC4 dest, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResize_8u_AC4R_Ctx(_devPtr, _pitch, _sizeOriginal, new NppiRect(_pointRoi, _sizeRoi), dest.DevicePointer, dest.Pitch, dest.Size, new NppiRect(dest.PointRoi, dest.SizeRoi), eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// resizes planar images.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void Resize(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer, dest3.DevicePointer };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResize_8u_P4R_Ctx(src, src0.Pitch, src0.Size, new NppiRect(src0.PointRoi, src0.SizeRoi), dst, dest0.Pitch, dest0.Size, new NppiRect(dest0.PointRoi, dest0.SizeRoi), eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Rotate images.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Rotate(NPPImage_8uC4 dest, double nAngle, double nShiftX, double nShiftY, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiRotate_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, new NppiRect(_pointRoi, _sizeRoi),
                dest.DevicePointer, dest.Pitch, new NppiRect(dest.PointRoi, dest.SizeRoi), nAngle, nShiftX, nShiftY, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRotate_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Rotate images. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nAngle">The angle of rotation in degrees.</param>
        /// <param name="nShiftX">Shift along horizontal axis</param>
        /// <param name="nShiftY">Shift along vertical axis</param>
        /// <param name="eInterpolation">Interpolation mode</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RotateA(NPPImage_8uC4 dest, double nAngle, double nShiftX, double nShiftY, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiRotate_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, new NppiRect(_pointRoi, _sizeRoi),
                dest.DevicePointer, dest.Pitch, new NppiRect(dest.PointRoi, dest.SizeRoi), nAngle, nShiftX, nShiftY, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRotate_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Mirror image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mirror(NPPImage_8uC4 dest, NppiAxis flip, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiMirror_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, flip, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Mirror image. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MirrorA(NPPImage_8uC4 dest, NppiAxis flip, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiMirror_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, flip, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion
        #region Affine Transformations

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffine(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffine_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. Not affecting Alpha channel.<para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffineA(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffine_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse affine transform of an image.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffineBack(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineBack_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse affine transform of an image. Not affecting Alpha channel.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffineBackA(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineBack_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffineQuad(double[,] srcQuad, NPPImage_8uC4 dest, double[,] dstQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineQuad_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, dstQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. Not affecting Alpha channel. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpAffineQuadA(double[,] srcQuad, NPPImage_8uC4 dest, double[,] dstQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineQuad_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, dstQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function operates using given transform coefficients that can be obtained
        /// by using nppiGetAffineTransform function or set explicitly. The function
        /// operates on source and destination regions of interest. The affine warp
        /// function transforms the source image pixel coordinates (x,y) according
        /// to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with 
        /// destination ROI specification.<para/>
        /// <para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but does not perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the
        /// fast one.<para/>
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes
        /// aligned. This is always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected 
        /// image are separated from the ROI by at least 63 bytes from each side.
        /// However, this requires the whole ROI to be part of allocated memory. In case
        /// when the conditions above are not satisfied, the function may decrease in
        /// speed slightly and will return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpAffine(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffine_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffine_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Inverse affine transform of an image.<para/>
        /// This function operates using given transform coefficients that can be
        /// obtained by using nppiGetAffineTransform function or set explicitly. Thus
        /// there is no need to invert coefficients in your application before calling
        /// WarpAffineBack. The function operates on source and destination regions of
        /// interest.<para/>
        /// The affine warp function transforms the source image pixel coordinates
        /// (x,y) according to the following formulas:<para/>
        /// X_new = C_00 * x + C_01 * y + C_02<para/>
        /// Y_new = C_10 * x + C_11 * y + C_12<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetAffineQuad and nppiGetAffineBound can help with
        /// destination ROI specification.<para/><para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Affine transform coefficients [2,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpAffineBack(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineBack_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBack_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Affine transform of an image. <para/>This
        /// function performs affine warping of a the specified quadrangle in the
        /// source image to the specified quadrangle in the destination image. The
        /// function nppiWarpAffineQuad uses the same formulas for pixel mapping as in
        /// nppiWarpAffine function. The transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// eInterpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dstQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpAffineQuad(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, double[,] srcQuad, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] dstQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.AffinTransforms.nppiWarpAffineQuad_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, srcQuad, dst, dest0.Pitch, rectOut, dstQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineQuad_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion
        #region Perspective Transformations

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspective(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspective_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image. Not affecting Alpha channel.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspectiveA(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspective_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse perspective transform of an image. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspectiveBack(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inverse perspective transform of an image. Not affecting Alpha channel. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspectiveBackA(NPPImage_8uC4 dest, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, dest.DevicePointer, dest.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspectiveQuad(double[,] srcQuad, NPPImage_8uC4 dest, double[,] destQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, destQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image. Not affecting Alpha channel.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest">Destination image</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void WarpPerspectiveQuadA(double[,] srcQuad, NPPImage_8uC4 dest, double[,] destQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect rectOut = new NppiRect(dest.PointRoi, dest.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, rectIn, srcQuad, dest.DevicePointer, dest.Pitch, rectOut, destQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. The function operates on source and destination regions 
        /// of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpPerspective(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspective_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspective_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Inverse perspective transform of an image. <para/>
        /// This function operates using given transform coefficients that 
        /// can be obtained by using nppiGetPerspectiveTransform function or set
        /// explicitly. Thus there is no need to invert coefficients in your application 
        /// before calling WarpPerspectiveBack. The function operates on source and
        /// destination regions of interest. The perspective warp function transforms the source image
        /// pixel coordinates (x,y) according to the following formulas:<para/>
        /// X_new = (C_00 * x + C_01 * y + C_02) / (C_20 * x + C_21 * y + C_22)<para/>
        /// Y_new = (C_10 * x + C_11 * y + C_12) / (C_20 * x + C_21 * y + C_22)<para/>
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.
        /// The functions nppiGetPerspectiveQuad and nppiGetPerspectiveBound can help
        /// with destination ROI specification.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="coeffs">Perspective transform coefficients [3,3]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpPerspectiveBack(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] coeffs, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveBack_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, dst, dest0.Pitch, rectOut, coeffs, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBack_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// Perspective transform of an image.<para/>
        /// This function performs perspective warping of a the specified
        /// quadrangle in the source image to the specified quadrangle in the
        /// destination image. The function nppiWarpPerspectiveQuad uses the same
        /// formulas for pixel mapping as in nppiWarpPerspective function. The
        /// transform coefficients are computed internally.
        /// The transformed part of the source image is resampled using the specified
        /// interpolation method and written to the destination ROI.<para/>
        /// NPPI specific recommendation: <para/>
        /// The function operates using 2 types of kernels: fast and accurate. The fast
        /// method is about 4 times faster than its accurate variant,
        /// but doesn't perform memory access checks and requires the destination ROI
        /// to be 64 bytes aligned. Hence any destination ROI is 
        /// chunked into 3 vertical stripes: the first and the third are processed by
        /// accurate kernels and the central one is processed by the fast one.
        /// In order to get the maximum available speed of execution, the projection of
        /// destination ROI onto image addresses must be 64 bytes aligned. This is
        /// always true if the values <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x))</code> and <para/>
        /// <code>(int)((void *)(pDst + dstRoi.x + dstRoi.width))</code> <para/>
        /// are multiples of 64. Another rule of thumb is to specify destination ROI in
        /// such way that left and right sides of the projected image are separated from
        /// the ROI by at least 63 bytes from each side. However, this requires the
        /// whole ROI to be part of allocated memory. In case when the conditions above
        /// are not satisfied, the function may decrease in speed slightly and will
        /// return NPP_MISALIGNED_DST_ROI_WARNING warning.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="srcQuad">Source quadrangle [4,2]</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="destQuad">Destination quadrangle [4,2]</param>
        /// <param name="eInterpolation">Interpolation mode: can be <see cref="InterpolationMode.NearestNeighbor"/>, <see cref="InterpolationMode.Linear"/> or <see cref="InterpolationMode.Cubic"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpPerspectiveQuad(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, double[,] srcQuad, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, double[,] destQuad, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect rectIn = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect rectOut = new NppiRect(dest0.PointRoi, dest0.SizeRoi);

            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer };

            NppStatus status = NPPNativeMethods_Ctx.NPPi.PerspectiveTransforms.nppiWarpPerspectiveQuad_8u_P4R_Ctx(src, src0.Size, src0.Pitch, rectIn, srcQuad, dst, dest0.Pitch, rectOut, destQuad, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveQuad_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Sum
        /// <summary>
        /// Scratch-buffer size for nppiSum_8u_C4R.
        /// </summary>
        /// <returns></returns>
        public int SumDoubleGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSumGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        /// <summary>
        /// Scratch-buffer size for nppiSum_8u_C4R. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int SumDoubleGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSumGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        /// <summary>
        /// Scratch-buffer size for nppiSum_8u_C4R.
        /// </summary>
        /// <returns></returns>
        public int SumLongGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSumGetBufferHostSize_8u64s_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumGetBufferHostSize_8u64s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image sum with 64-bit double precision result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sum(CudaDeviceVariable<double> result, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumDoubleGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SumDoubleGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sum(CudaDeviceVariable<double> result, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumDoubleGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumA(CudaDeviceVariable<double> result, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumDoubleGetBufferHostSizeA(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SumDoubleGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumA(CudaDeviceVariable<double> result, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumDoubleGetBufferHostSizeA(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit long long result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(long)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sum(CudaDeviceVariable<long> result, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumLongGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u64s_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u64s_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image sum with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="result">Allocated device memory with size of at least 4 * sizeof(long)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SumLongGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Sum(CudaDeviceVariable<long> result, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SumLongGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Sum.nppiSum_8u64s_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, result.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSum_8u64s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region LUT
        /// <summary>
        /// look-up-table color conversion.<para/>
        /// The LUT is derived from a set of user defined mapping points through linear interpolation. Not affecting alpha channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="values0">array of user defined OUTPUT values, channel 0</param>
        /// <param name="levels0">array of user defined INPUT values, channel 0</param>
        /// <param name="values1">array of user defined OUTPUT values, channel 1</param>
        /// <param name="levels1">array of user defined INPUT values, channel 1</param>
        /// <param name="values2">array of user defined OUTPUT values, channel 2</param>
        /// <param name="levels2">array of user defined INPUT values, channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LutA(NPPImage_8uC4 dest, CudaDeviceVariable<int> values0, CudaDeviceVariable<int> levels0, CudaDeviceVariable<int> values1,
            CudaDeviceVariable<int> levels1, CudaDeviceVariable<int> values2, CudaDeviceVariable<int> levels2, NppStreamContext nppStreamCtx)
        {
            if (values0.Size != levels0.Size) throw new ArgumentException("values0 and levels0 must have same size.");
            if (values1.Size != levels1.Size) throw new ArgumentException("values1 and levels1 must have same size.");
            if (values2.Size != levels2.Size) throw new ArgumentException("values2 and levels2 must have same size.");

            CUdeviceptr[] values = new CUdeviceptr[3];
            CUdeviceptr[] levels = new CUdeviceptr[3];
            int[] levelLengths = new int[3];

            values[0] = values0.DevicePointer;
            values[1] = values1.DevicePointer;
            values[2] = values2.DevicePointer;

            levels[0] = levels0.DevicePointer;
            levels[1] = levels1.DevicePointer;
            levels[2] = levels2.DevicePointer;

            levelLengths[0] = levels0.Size;
            levelLengths[1] = levels1.Size;
            levelLengths[2] = levels2.Size;

            status = NPPNativeMethods_Ctx.NPPi.ColorProcessing.nppiLUT_Linear_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, values, levels, levelLengths, nppStreamCtx);

            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// look-up-table color conversion.<para/>
        /// The LUT is derived from a set of user defined mapping points through linear interpolation.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="values0">array of user defined OUTPUT values, channel 0</param>
        /// <param name="levels0">array of user defined INPUT values, channel 0</param>
        /// <param name="values1">array of user defined OUTPUT values, channel 1</param>
        /// <param name="levels1">array of user defined INPUT values, channel 1</param>
        /// <param name="values2">array of user defined OUTPUT values, channel 2</param>
        /// <param name="levels2">array of user defined INPUT values, channel 2</param>
        /// <param name="values3">array of user defined OUTPUT values, channel 3</param>
        /// <param name="levels3">array of user defined INPUT values, channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Lut(NPPImage_8uC4 dest, CudaDeviceVariable<int> values0, CudaDeviceVariable<int> levels0, CudaDeviceVariable<int> values1, CudaDeviceVariable<int> levels1,
            CudaDeviceVariable<int> values2, CudaDeviceVariable<int> levels2, CudaDeviceVariable<int> values3, CudaDeviceVariable<int> levels3, NppStreamContext nppStreamCtx)
        {
            if (values0.Size != levels0.Size) throw new ArgumentException("values0 and levels0 must have same size.");
            if (values1.Size != levels1.Size) throw new ArgumentException("values1 and levels1 must have same size.");
            if (values2.Size != levels2.Size) throw new ArgumentException("values2 and levels2 must have same size.");
            if (values3.Size != levels3.Size) throw new ArgumentException("values3 and levels3 must have same size.");

            CUdeviceptr[] values = new CUdeviceptr[4];
            CUdeviceptr[] levels = new CUdeviceptr[4];
            int[] levelLengths = new int[4];

            values[0] = values0.DevicePointer;
            values[1] = values1.DevicePointer;
            values[2] = values2.DevicePointer;
            values[3] = values3.DevicePointer;

            levels[0] = levels0.DevicePointer;
            levels[1] = levels1.DevicePointer;
            levels[2] = levels2.DevicePointer;
            levels[3] = levels3.DevicePointer;

            levelLengths[0] = levels0.Size;
            levelLengths[1] = levels1.Size;
            levelLengths[2] = levels2.Size;
            levelLengths[3] = levels3.Size;

            status = NPPNativeMethods_Ctx.NPPi.ColorProcessing.nppiLUT_Linear_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, values, levels, levelLengths, nppStreamCtx);

            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Color conversion
        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB to packed YCbCr color conversion. Alpha channel is the last channel and is not processed.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYCbCr(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToYCbCr.nppiRGBToYCbCr_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYCbCr_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed YCbCr to RGB color conversion. Alpha channel is the last channel and is not processed.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void YCbCrToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.YCbCrToRGB.nppiYCbCrToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYCbCrToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Alpha Composition

        /// <summary>
        /// Four 8-bit unsigned char channel image composition using image alpha values (0 - max channel pixel value).
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaComp(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppiAlphaOp nppAlphaOp, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaComp.nppiAlphaComp_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaComp_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image composition using constant alpha.
        /// </summary>
        /// <param name="alpha1">constant alpha for this image</param>
        /// <param name="src2">2nd source image</param>
        /// <param name="alpha2">constant alpha for src2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaComp(byte alpha1, NPPImage_8uC4 src2, byte alpha2, NPPImage_8uC4 dest, NppiAlphaOp nppAlphaOp, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaCompConst.nppiAlphaCompC_8u_C4R_Ctx(_devPtrRoi, _pitch, alpha1, src2.DevicePointerRoi, src2.Pitch, alpha2, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaCompC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image composition using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha1">constant alpha for this image</param>
        /// <param name="src2">2nd source image</param>
        /// <param name="alpha2">constant alpha for src2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppAlphaOp">alpha compositing operation</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaCompA(byte alpha1, NPPImage_8uC4 src2, byte alpha2, NPPImage_8uC4 dest, NppiAlphaOp nppAlphaOp, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaCompConst.nppiAlphaCompC_8u_AC4R_Ctx(_devPtrRoi, _pitch, alpha1, src2.DevicePointerRoi, src2.Pitch, alpha2, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppAlphaOp, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaCompC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image premultiplication using constant alpha.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaPremul(byte alpha, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremulConst.nppiAlphaPremulC_8u_C4R_Ctx(_devPtrRoi, _pitch, alpha, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// In place alpha premultiplication using constant alpha.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaPremul(byte alpha, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremulConst.nppiAlphaPremulC_8u_C4IR_Ctx(alpha, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image premultiplication using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaPremulA(byte alpha, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremulConst.nppiAlphaPremulC_8u_AC4R_Ctx(_devPtrRoi, _pitch, alpha, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// In place alpha premultiplication using constant alpha. Not affecting alpha channel.
        /// </summary>
        /// <param name="alpha">alpha</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaPremulA(byte alpha, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremulConst.nppiAlphaPremulC_8u_AC4IR_Ctx(alpha, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremulC_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four 8-bit unsigned char channel image premultiplication with pixel alpha (0 - max channel pixel value).
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaPremulA(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremul.nppiAlphaPremul_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremul_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four 8-bit unsigned char channel in place image premultiplication with pixel alpha (0 - max channel pixel value).
        /// </summary>
        public void AlphaPremulA(NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AlphaPremul.nppiAlphaPremul_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaPremul_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Compare
        /// <summary>
        /// Compare pSrc1’s pixels with corresponding pixels in pSrc2.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Compare(NPPImage_8uC4 src2, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Compare.nppiCompare_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompare_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Compare pSrc's pixels with constant value.
        /// </summary>
        /// <param name="constants">list of constants, one per color channel.</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Compare(byte[] constants, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Compare.nppiCompareC_8u_C4R_Ctx(_devPtrRoi, _pitch, constants, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompareC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Compare pSrc1’s pixels with corresponding pixels in pSrc2. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CompareA(NPPImage_8uC4 src2, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Compare.nppiCompare_8u_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompare_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Compare pSrc's pixels with constant value. Not affecting Alpha.
        /// </summary>
        /// <param name="constants">list of constants, one per color channel.</param>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">Specifies the comparison operation to be used in the pixel comparison.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CompareA(byte[] constants, NPPImage_8uC1 dest, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Compare.nppiCompareC_8u_AC4R_Ctx(_devPtrRoi, _pitch, constants, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompareC_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Threshold

        /// <summary>
        /// If for a comparison operations OP the predicate (sourcePixel.channel OP nThreshold) is true, the channel
        /// value is set to nThreshold, otherwise it is set to sourcePixel. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eComparisonOperation">The type of comparison operation to be used. The only valid values are: NppCmpOp.Less and NppCmpOp.Greater</param>
        /// <param name="aThresholds">The threshold values, one per color channel. (Size = 3)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Threshold(NPPImage_8uC4 dest, NppCmpOp eComparisonOperation, byte[] aThresholds, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aThresholds, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Swap Channels

        /// <summary>
        /// Swap channels, in-place.
        /// </summary>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// contains the number of the channel that is stored in the n-th channel of the output image. E.g.
        /// Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR channel order.</param>
        public void SwapChannels(int[] aDstOrder, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SwapChannel.nppiSwapChannels_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, aDstOrder, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Swap channels.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Host memory integer array describing how channel values are permutated. The n-th entry
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// of the array contains the number of the channel that is stored in the n-th channel of
        /// the output image. <para/>E.g. Given an RGBA image, aDstOrder = [2,1,0] converts this to a 3 channel BGR
        /// channel order.</param>
        public void SwapChannels(NPPImage_8uC3 dest, int[] aDstOrder, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SwapChannel.nppiSwapChannels_8u_C4C3R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_8u_C4C3R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Swap channels.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// contains the number of the channel that is stored in the n-th channel of the output image. E.g.
        /// Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR channel order.</param>
        public void SwapChannels(NPPImage_8uC4 dest, int[] aDstOrder, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SwapChannel.nppiSwapChannels_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Swap channels. Not affecting Alpha
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aDstOrder">Integer array describing how channel values are permutated. The n-th entry of the array contains the number of the channel that is stored in the n-th channel of
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// the output image. E.g. Given an RGBA image, aDstOrder = [2,1,0] converts this to BGRA
        /// channel order. In the AC4R case, the alpha channel is always assumed to be channel 3.
        /// </param>
        public void SwapChannelsA(NPPImage_8uC4 dest, int[] aDstOrder, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.SwapChannel.nppiSwapChannels_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aDstOrder, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSwapChannels_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Convert
        /// <summary>
        /// 8-bit unsigned to 16-bit signed conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Convert(NPPImage_16sC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u16s_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u16s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 8-bit unsigned to 16-bit unsigned conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Convert(NPPImage_16uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u16u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u16u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 8-bit unsigned to 16-bit signed conversion. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ConvertA(NPPImage_16sC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u16s_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u16s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 8-bit unsigned to 16-bit unsigned conversion. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ConvertA(NPPImage_16uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u16u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u16u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 8-bit unsigned to 32-bit floating point conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Convert(NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u32f_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 8-bit unsigned to 32-bit signed conversion.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Convert(NPPImage_32sC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u32s_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u32s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 8-bit unsigned to 32-bit floating point conversion. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ConvertA(NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 8-bit unsigned to 32-bit signed conversion. Not affecting Alpha channel.
        /// </summary>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ConvertA(NPPImage_32sC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BitDepthConversion.nppiConvert_8u32s_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiConvert_8u32s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region AbsDiff
        /// <summary>
        /// Absolute difference of this minus src2.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AbsDiff(NPPImage_8uC4 src2, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.AbsDiff.nppiAbsDiff_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAbsDiff_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region CompColorKey
        /// <summary>
        /// packed color complement color key replacement of source image 1 by source image 2
        /// </summary>
        /// <param name="src2">source2 packed pixel format image.</param>
        /// <param name="dest">Destination image</param>
        /// <param name="colorKeyConst">color key constants</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CompColorKey(NPPImage_8uC4 src2, NPPImage_8uC4 dest, byte[] colorKeyConst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CompColorKey.nppiCompColorKey_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, colorKeyConst, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCompColorKey_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Min
        /// <summary>
        /// Scratch-buffer size for Min.
        /// </summary>
        /// <returns></returns>
        public int MinGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMinGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Min(CudaDeviceVariable<byte> min, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMin_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Min(CudaDeviceVariable<byte> min, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMin_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Scratch-buffer size for Min. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int MinGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMinGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed. Not affecting alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinA(CudaDeviceVariable<byte> min, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMin_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinA(CudaDeviceVariable<byte> min, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Min.nppiMin_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMin_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinIndex
        /// <summary>
        /// Scratch-buffer size for MinIndex.
        /// </summary>
        /// <returns></returns>
        public int MinIndexGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndxGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndxGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinIndex(CudaDeviceVariable<byte> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinIndexGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndx_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinIndexGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinIndex(CudaDeviceVariable<byte> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinIndexGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndx_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for MinIndex. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int MinIndexGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndxGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndxGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel minimum. Buffer is internally allocated and freed. Not affecting alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinIndexA(CudaDeviceVariable<byte> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinIndexGetBufferHostSizeA(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndx_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting alpha.
        /// </summary>
        /// <param name="min">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MinIndexGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinIndexA(CudaDeviceVariable<byte> min, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MinIndexGetBufferHostSizeA(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MinIdx.nppiMinIndx_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, min.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinIndx_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Max
        /// <summary>
        /// Scratch-buffer size for Max.
        /// </summary>
        /// <returns></returns>
        public int MaxGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMaxGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Max(CudaDeviceVariable<byte> max, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMax_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel maximum. No additional buffer is allocated.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Max(CudaDeviceVariable<byte> max, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMax_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Max. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int MaxGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMaxGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed. Not affecting alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxA(CudaDeviceVariable<byte> max, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxGetBufferHostSizeA(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMax_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel maximum. No additional buffer is allocated. Not affecting alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxA(CudaDeviceVariable<byte> max, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxGetBufferHostSizeA(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.Max.nppiMax_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMax_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MaxIndex
        /// <summary>
        /// Scratch-buffer size for MaxIndex.
        /// </summary>
        /// <returns></returns>
        public int MaxIndexGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndxGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndxGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxIndex(CudaDeviceVariable<byte> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxIndexGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndx_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 4 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 4 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxIndexGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxIndex(CudaDeviceVariable<byte> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxIndexGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndx_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for MaxIndex. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int MaxIndexGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndxGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndxGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Image pixel maximum. Buffer is internally allocated and freed. Not affecting alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxIndexA(CudaDeviceVariable<byte> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxIndexGetBufferHostSizeA(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndx_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Image pixel minimum. No additional buffer is allocated. Not affecting alpha.
        /// </summary>
        /// <param name="max">Allocated device memory with size of at least 3 * sizeof(byte)</param>
        /// <param name="indexX">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="indexY">Allocated device memory with size of at least 3 * sizeof(int)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MaxIndexGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxIndexA(CudaDeviceVariable<byte> max, CudaDeviceVariable<int> indexX, CudaDeviceVariable<int> indexY, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxIndexGetBufferHostSizeA(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MaxIdx.nppiMaxIndx_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, max.DevicePointer, indexX.DevicePointer, indexY.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxIndx_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Mean
        /// <summary>
        /// Scratch-buffer size for Mean.
        /// </summary>
        /// <returns></returns>
        public int MeanGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMeanGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMeanGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image mean with 64-bit double precision result. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mean(CudaDeviceVariable<double> mean, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MeanGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMean_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image mean with 64-bit double precision result. No additional buffer is allocated.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MeanGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mean(CudaDeviceVariable<double> mean, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MeanGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMean_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Scratch-buffer size for Mean. Not affecting alpha.
        /// </summary>
        /// <returns></returns>
        public int MeanGetBufferHostSizeA(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMeanGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMeanGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image mean with 64-bit double precision result. Buffer is internally allocated and freed. Not affecting alpha.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MeanA(CudaDeviceVariable<double> mean, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MeanGetBufferHostSizeA(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMean_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image mean with 64-bit double precision result. No additional buffer is allocated. Not affecting alpha.
        /// </summary>
        /// <param name="mean">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="MeanGetBufferHostSizeA()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MeanA(CudaDeviceVariable<double> mean, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MeanGetBufferHostSizeA(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MeanNew.nppiMean_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, buffer.DevicePointer, mean.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMean_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Gamma
        /// <summary>
        /// image forward gamma correction. Not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void GammaA(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Gamma.nppiGammaFwd_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGammaFwd_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image forward gamma correction. Not affecting alpha.
        /// </summary>
        public void GammaA(NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Gamma.nppiGammaFwd_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGammaFwd_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region GammaInv
        /// <summary>
        /// image inverse gamma correction. Not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void GammaInvA(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Gamma.nppiGammaInv_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGammaInv_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace image inverse gamma correction. Not affecting alpha.
        /// </summary>
        public void GammaInvA(NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Gamma.nppiGammaInv_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiGammaInv_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region AlphaCompColorKey
        /// <summary>
        /// complement color key replacement of source image 1 by source image 2 with alpha blending.
        /// </summary>
        /// <param name="alpha1">source1 image alpha opacity (0 - max channel pixel value).</param>
        /// <param name="src2">source2 packed pixel format image.</param>
        /// <param name="alpha2">source2 image alpha opacity (0 - max channel pixel value).</param>
        /// <param name="dest">Destination image</param>
        /// <param name="colorKeyConst">color key constants</param>
        /// <param name="nppAlphaOp">NppiAlphaOp alpha compositing operation selector  (excluding premul ops).</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AlphaCompColorKeyA(byte alpha1, NPPImage_8uC4 src2, byte alpha2, NPPImage_8uC4 dest, byte[] colorKeyConst, NppiAlphaOp nppAlphaOp, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CompColorKey.nppiAlphaCompColorKey_8u_AC4R_Ctx(_devPtrRoi, _pitch, alpha1, src2.DevicePointerRoi, src2.Pitch, alpha2, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, colorKeyConst, nppAlphaOp, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAlphaCompColorKey_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormInf
        /// <summary>
        /// Scratch-buffer size for Norm inf.
        /// </summary>
        /// <returns></returns>
        public int NormInfGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormInf.nppiNormInfGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormInfGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image infinity norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormInf(CudaDeviceVariable<double> norm, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormInfGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormInf.nppiNorm_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image infinity norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormInfGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormInf(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormInfGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormInf.nppiNorm_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_Inf_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormL1
        /// <summary>
        /// Scratch-buffer size for Norm L1.
        /// </summary>
        /// <returns></returns>
        public int NormL1GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormL1.nppiNormL1GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL1GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L1 norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormL1(CudaDeviceVariable<double> norm, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormL1GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormL1.nppiNorm_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L1 norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL1GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormL1(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormL1GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormL1.nppiNorm_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L1_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region NormL2
        /// <summary>
        /// Scratch-buffer size for Norm L2.
        /// </summary>
        /// <returns></returns>
        public int NormL2GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormL2.nppiNormL2GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormL2GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image L2 norm. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormL2(CudaDeviceVariable<double> norm, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormL2GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormL2.nppiNorm_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image L2 norm. No additional buffer is allocated.
        /// </summary>
        /// <param name="norm">Allocated device memory with size of at least 3 * sizeof(double)</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormL2GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormL2(CudaDeviceVariable<double> norm, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormL2GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormL2.nppiNorm_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, norm.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNorm_L2_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Threshold
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Threshold(NPPImage_8uC4 dest, byte[] nThreshold, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Threshold(byte[] nThreshold, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdGT
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdGT(NPPImage_8uC4 dest, byte[] nThreshold, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_GT_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GT_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdGT(byte[] nThreshold, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_GT_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GT_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLT
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLT(NPPImage_8uC4 dest, byte[] nThreshold, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LT_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LT_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nThreshold, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLT(byte[] nThreshold, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LT_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LT_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdVal
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Threshold(NPPImage_8uC4 dest, byte[] nThreshold, byte[] nValue, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_Val_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_Val_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations OP the predicate (sourcePixel OP nThreshold) is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="eComparisonOperation">eComparisonOperation. Only allowed values are <see cref="NppCmpOp.Less"/> and <see cref="NppCmpOp.Greater"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Threshold(byte[] nThreshold, byte[] nValue, NppCmpOp eComparisonOperation, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_Val_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue, eComparisonOperation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_Val_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdGTVal
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdGT(NPPImage_8uC4 dest, byte[] nThreshold, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_GTVal_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GTVal_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations sourcePixel is greater than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdGT(byte[] nThreshold, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_GTVal_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_GTVal_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLTVal
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLT(NPPImage_8uC4 dest, byte[] nThreshold, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LTVal_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThreshold, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTVal_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThreshold is true, the pixel is set
        /// to nValue, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThreshold">The threshold value.</param>
        /// <param name="nValue">The threshold replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLT(byte[] nThreshold, byte[] nValue, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LTVal_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThreshold, nValue, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTVal_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region ThresholdLTValGTVal
        /// <summary>
        /// Image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThresholdLT is true, the pixel is set
        /// to nValueLT, else if sourcePixel is greater than nThresholdGT the pixel is set to nValueGT, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nThresholdLT">The thresholdLT value.</param>
        /// <param name="nValueLT">The thresholdLT replacement value.</param>
        /// <param name="nThresholdGT">The thresholdGT value.</param>
        /// <param name="nValueGT">The thresholdGT replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLTGT(NPPImage_8uC4 dest, byte[] nThresholdLT, byte[] nValueLT, byte[] nThresholdGT, byte[] nValueGT, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LTValGTVal_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nThresholdLT, nValueLT, nThresholdGT, nValueGT, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTValGTVal_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// In place image threshold.<para/>
        /// If for a comparison operations sourcePixel is less than nThresholdLT is true, the pixel is set
        /// to nValueLT, else if sourcePixel is greater than nThresholdGT the pixel is set to nValueGT, otherwise it is set to sourcePixel.
        /// </summary>
        /// <param name="nThresholdLT">The thresholdLT value.</param>
        /// <param name="nValueLT">The thresholdLT replacement value.</param>
        /// <param name="nThresholdGT">The thresholdGT value.</param>
        /// <param name="nValueGT">The thresholdGT replacement value.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ThresholdLTGT(byte[] nThresholdLT, byte[] nValueLT, byte[] nThresholdGT, byte[] nValueGT, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Threshold.nppiThreshold_LTValGTVal_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, nThresholdLT, nValueLT, nThresholdGT, nValueGT, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiThreshold_LTValGTVal_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region BGRToHLS
        /// <summary>
        /// 4 channel 8-bit unsigned planar HLS with alpha to 4 channel 8-bit unsigned packed BGR with alpha color conversion.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void BGRToHLS(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToHLS.nppiBGRToHLS_8u_AP4C4R_Ctx(array, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToHLS_8u_AP4C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned planar BGR with alpha to 4 channel 8-bit unsigned planar HLS with alpha color conversion.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void BGRToHLS(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToHLS.nppiBGRToHLS_8u_AP4R_Ctx(arraySrc, src0.Pitch, arrayDest, dest0.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToHLS_8u_AP4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region HLSToBGR
        /// <summary>
        /// 4 channel 8-bit unsigned planar HLS with alpha to 4 channel 8-bit unsigned planar BGR with alpha color conversion.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void HLSToBGR(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.HLSToBGR.nppiHLSToBGR_8u_AP4R_Ctx(arraySrc, src0.Pitch, arrayDest, dest0.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHLSToBGR_8u_AP4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned planar HLS with alpha to 4 channel 8-bit unsigned packed BGR with alpha color conversion.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="src3">Source image channel 2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void HLSToBGR(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] array = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.HLSToBGR.nppiHLSToBGR_8u_AP4C4R_Ctx(array, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHLSToBGR_8u_AP4C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region Color Space conversion new in CUDA 5
        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned packed YUV color conversion with alpha, not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYUV(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToYUV.nppiRGBToYUV_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYUV_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit packed YUV with alpha to 4 channel 8-bit unsigned packed RGB color conversion with alpha, not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void YUVToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.YUVToRGB.nppiYUVToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYUVToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 2 channel 8-bit unsigned packed YCrCb422 color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr422(NPPImage_8uC2 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr422_8u_AC4C2R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr422_8u_AC4C2R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 2 channel 8-bit unsigned packed CbYCr422 color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToCbYCr422(NPPImage_8uC2 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BGRToCbYCr.nppiBGRToCbYCr422_8u_AC4C2R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToCbYCr422_8u_AC4C2R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 2 channel 8-bit unsigned packed CbYCr422_709HDTV color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToCbYCr422_709HDTV(NPPImage_8uC2 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BGRToCbYCr.nppiBGRToCbYCr422_709HDTV_8u_AC4C2R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToCbYCr422_709HDTV_8u_AC4C2R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned packed XYZ with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToXYZ(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToXYZ.nppiRGBToXYZ_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToXYZ_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed XYZ with alpha to 4 channel 8-bit unsigned packed RGB with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void XYZToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.XYZToRGB.nppiXYZToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiXYZToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned packed LUV with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToLUV(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToLUV.nppiRGBToLUV_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToLUV_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed LUV with alpha to 4 channel 8-bit unsigned packed RGB with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUVToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LUVToRGB.nppiLUVToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUVToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed YCC with alpha to 4 channel 8-bit unsigned packed RGB with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void YCCToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.YCCToRGB.nppiYCCToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYCCToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned packed HLS with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToHLS(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToHLS.nppiRGBToHLS_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToHLS_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed HLS with alpha to 4 channel 8-bit unsigned packed RGB with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HLSToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.HLSToRGB.nppiHLSToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHLSToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 4 channel 8-bit unsigned packed HLS with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToHLS(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BGRToHLS.nppiBGRToHLS_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToHLS_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        ///// <summary>
        ///// 4 channel 8-bit unsigned packed HLS with alpha to 4 channel 8-bit unsigned packed BGR with alpha color conversion.
        ///// </summary>
        ///// <param name="dest">Destination image</param>
        //public void HLSToBGR(NPPImage_8uC4 dest)
        //{
        //	status = NPPNativeMethods_Ctx.NPPi.HLSToBGR.nppiHLSToBGR_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHLSToBGR_8u_AC4R_Ctx", status));
        //	NPPException.CheckNppStatus(status, this);
        //}

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned packed HSV with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToHSV(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToHSV.nppiRGBToHSV_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToHSV_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed HSV with alpha to 4 channel 8-bit unsigned packed RGB with alpha color conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HSVToRGB(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.HSVToRGB.nppiHSVToRGB_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHSVToRGB_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 3 channel 8-bit unsigned planar YCbCr color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYCbCr(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.RGBToYCbCr.nppiRGBToYCbCr_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYCbCr_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 4 channel 8-bit unsigned planar YUV color conversion with alpha.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYUV(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.RGBToYUV.nppiRGBToYUV_8u_AC4P4R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYUV_8u_AC4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 4 channel 8-bit unsigned planar HLS with alpha color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToHLS(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToHLS.nppiBGRToHLS_8u_AC4P4R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToHLS_8u_AC4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed HLS with alpha to 4 channel 8-bit unsigned planar BGR with alpha color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void HLSToBGR(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.HLSToBGR.nppiHLSToBGR_8u_AC4P4R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiHLSToBGR_8u_AC4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned pacmed BGR with alpha to 3 channel 8-bit unsigned planar YUV420 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYUV420(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYUV420.nppiBGRToYUV420_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYUV420_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr422 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr422(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr422_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr422_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 3 channel 8-bit unsigned planar YCrCb420 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYCrCb420(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.RGBToYCrCb.nppiRGBToYCrCb420_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYCrCb420_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr420 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr420(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr420_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr420_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr420_709CSC color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr420_709CSC(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr420_709CSC_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr420_709CSC_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr420_709HDTV color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr420_709HDTV(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr420_709HDTV_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr420_709HDTV_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCrCb420_709CSC color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCrCb420_709CSC(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCrCb.nppiBGRToYCrCb420_709CSC_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCrCb420_709CSC_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCrCb420 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCrCb420(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCrCb.nppiBGRToYCrCb420_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCrCb420_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr411 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr411(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr411_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, arrayPitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr411_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        //new in Cuda 5.5
        #region DotProduct
        /// <summary>
        /// Device scratch buffer size (in bytes) for nppiDotProd_8u64f_C4R.
        /// </summary>
        /// <returns></returns>
        public int DotProdGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProdGetBufferHostSize_8u64f_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProdGetBufferHostSize_8u64f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Four-channel 8-bit unsigned image DotProd.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (4 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="DotProdGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DotProduct(NPPImage_8uC4 src2, CudaDeviceVariable<double> pDp, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = DotProdGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProd_8u64f_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_8u64f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four-channel 8-bit unsigned image DotProd. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (4 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DotProduct(NPPImage_8uC4 src2, CudaDeviceVariable<double> pDp, NppStreamContext nppStreamCtx)
        {
            int bufferSize = DotProdGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProd_8u64f_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_8u64f_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for nppiDotProd_8u64f_C4R. Ignoring alpha channel.
        /// </summary>
        /// <returns></returns>
        public int ADotProdGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProdGetBufferHostSize_8u64f_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProdGetBufferHostSize_8u64f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// Four-channel 8-bit unsigned image DotProd. Ignoring alpha channel.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ADotProdGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ADotProduct(NPPImage_8uC4 src2, CudaDeviceVariable<double> pDp, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = DotProdGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProd_8u64f_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_8u64f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Four-channel 8-bit unsigned image DotProd. Buffer is internally allocated and freed. Ignoring alpha channel.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pDp">Pointer to the computed dot product of the two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ADotProduct(NPPImage_8uC4 src2, CudaDeviceVariable<double> pDp, NppStreamContext nppStreamCtx)
        {
            int bufferSize = DotProdGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.DotProd.nppiDotProd_8u64f_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pDp.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDotProd_8u64f_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region LUT

        /// <summary>
        /// look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUT(NPPImage_8uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUT.nppiLUT_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTCubic(NPPImage_8uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTCubic.nppiLUT_Cubic_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// range restricted palette look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points in a palette and 
        /// source pixels are then processed using a restricted bit range when looking up palette values.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pTable">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT palette values.</param>
        /// <param name="nBitSize">Number of least significant bits (must be &gt; 0 and &lt;= 8) of each source pixel value to use as index into palette table during conversion.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTPalette(NPPImage_8uC4 dst, CudaDeviceVariable<byte>[] pTable, int nBitSize, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsT = new CUdeviceptr[] { pTable[0].DevicePointer, pTable[1].DevicePointer, pTable[2].DevicePointer, pTable[3].DevicePointer };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTPalette.nppiLUTPalette_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsT, nBitSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUTPalette_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUT(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUT.nppiLUT_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTCubic(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTCubic.nppiLUT_Cubic_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace linear interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation. 
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 4 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTLinear(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer, pValues[3].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer, pLevels[3].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size, pLevels[3].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTLinear.nppiLUT_Linear_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Transpose
        /// <summary>
        /// image transpose
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Transpose(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Transpose.nppiTranspose_8u_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiTranspose_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Copy

        /// <summary>
        /// image copy with nearest source image pixel color.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyReplicateBorder(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyReplicateBorder.nppiCopyReplicateBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyReplicateBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image copy with the borders wrapped by replication of source image pixel colors.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyWrapBorder(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyWrapBorder.nppiCopyWrapBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyWrapBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// linearly interpolated source image subpixel coordinate color copy.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nDx">Fractional part of source image X coordinate.</param>
        /// <param name="nDy">Fractional part of source image Y coordinate.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CopySubpix(NPPImage_8uC4 dst, float nDx, float nDy, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopySubpix.nppiCopySubpix_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nDx, nDy, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopySubpix_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMaxEveryNew
        /// <summary>
        /// image MinEvery
        /// </summary>
        /// <param name="src2">Source-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinEvery(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MinMaxEvery.nppiMinEvery_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinEvery_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image MaxEvery
        /// </summary>
        /// <param name="src2">Source-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxEvery(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MinMaxEvery.nppiMaxEvery_8u_C4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxEvery_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MirrorNew


        /// <summary>
        /// Mirror image inplace.
        /// </summary>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Mirror(NppiAxis flip, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiMirror_8u_C4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, flip, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MorphologicalNew
        /// <summary>
        /// 3x3 dilation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Dilate3x3(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiDilate3x3_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Erode3x3(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiErode3x3_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter
        /// <summary>
        /// 1D column convolution.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumn(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumn32f_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 1D row convolution.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRow(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRow32f_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Filter(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter32f_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Filter(NPPImage_16sC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter32f_8u16s_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_8u16s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittHoriz(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittHoriz_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittHoriz_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittVert(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittVert_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittVert_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Sobel filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SobelHoriz(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSobelHoriz_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelHoriz_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Sobel filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelVert(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSobelVert_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelVert_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// horizontal Roberts filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsDown(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsDown_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsDown_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// vertical Roberts filter..
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsUp(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsUp_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsUp_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Laplace filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLaplace(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLaplace_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLaplace_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Gauss filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGauss(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterGauss_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGauss_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterHighPass(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterHighPass_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPass_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLowPass(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLowPass_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPass_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSharpen(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSharpen_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpen_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region NormNew
        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_Inf.
        /// </summary>
        /// <returns></returns>
        public int NormDiffInfGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffInfGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffInfGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_Inf.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffInfGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_Inf(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffInfGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_Inf_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_Inf. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_Inf(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffInfGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_Inf_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L1.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL1GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffL1GetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL1GetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L1.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL1GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L1(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL1GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L1_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L1. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L1(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL1GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L1_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L2.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL2GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffL2GetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL2GetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L2.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL2GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L2(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL2GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L2_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L2. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L2(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL2GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L2_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_Inf.
        /// </summary>
        /// <returns></returns>
        public int NormRelInfGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelInfGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelInfGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_Inf.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelInfGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_Inf(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelInfGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_Inf_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_Inf. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_Inf(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelInfGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_Inf_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L1.
        /// </summary>
        /// <returns></returns>
        public int NormRelL1GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelL1GetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL1GetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L1.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL1GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L1(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL1GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L1_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L1. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L1(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL1GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L1_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L2.
        /// </summary>
        /// <returns></returns>
        public int NormRelL2GetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelL2GetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL2GetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L2.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL2GetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L2(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL2GetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L2_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L2. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L2(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL2GetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L2_8u_C4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }





        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_8u32f_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u32f_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_8u32f_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u32f_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_8u32f_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevel(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u32f_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }






        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelScaledGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_8u_C4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelScaledGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelScaledGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelScaledGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u_C4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelScaledGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_8u_C4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelScaledGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelScaledGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelScaledGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u_C4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelScaledGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_8u_C4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelScaledGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelScaledGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevel(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelScaledGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u_C4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }






        /// <summary>
        /// image SqrDistanceFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceFull_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceSame_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceValid_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// image SqrDistanceFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceFull_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceSame_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceValid_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// image CrossCorrFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_Norm(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_Norm_8u32f_C4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// image CrossCorrFull_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_Norm(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_Norm_8u_C4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_8u_C4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region GeometryNew

        /// <summary>
        /// image resize.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ResizeSqrPixel(NPPImage_8uC4 dst, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect dstRect = new NppiRect(dst.PointRoi, dst.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, srcRect, dst.DevicePointer, dst.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image remap.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Remap(NPPImage_8uC4 dst, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Remap.nppiRemap_8u_C4R_Ctx(_devPtr, _sizeOriginal, _pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Scale(NPPImage_16sC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u16s_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u16s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// image conversion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Scale(NPPImage_16uC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u16u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u16u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nMin">specifies the minimum saturation value to which every output value will be clamped.</param>
        /// <param name="nMax">specifies the maximum saturation value to which every output value will be clamped.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Scale(NPPImage_32fC4 dst, float nMin, float nMax, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u32f_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nMin, nMax, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Scale(NPPImage_32sC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u32s_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u32s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion


        //Alpha
        #region Color...New

        /// <summary>
        /// RGB to Gray conversion, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToGrayA(NPPImage_8uC1 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RGBToGray.nppiRGBToGray_8u_AC4C1R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToGray_8u_AC4C1R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Color to Gray conversion, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aCoeffs">fixed size array of constant floating point conversion coefficient values, one per color channel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorToGrayA(NPPImage_8uC1 dest, float[] aCoeffs, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorToGray.nppiColorToGray_8u_AC4C1R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aCoeffs, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorToGray_8u_AC4C1R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Color to Gray conversion.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="aCoeffs">fixed size array of constant floating point conversion coefficient values, one per color channel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorToGray(NPPImage_8uC1 dest, float[] aCoeffs, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorToGray.nppiColorToGray_8u_C4C1R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, aCoeffs, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorToGray_8u_C4C1R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// in place color twist, not affecting Alpha.
        /// 
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within ROI.
        /// </summary>
        /// <param name="aTwist">The color twist matrix with floating-point coefficient values. [3,4]</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ColorTwistA(float[,] aTwist, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ColorTwist.nppiColorTwist32f_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, aTwist, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwist32f_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MorphologicalNew
        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DilateA(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiDilate_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search, not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ErodeA(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiErode_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 3x3 dilation, not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Dilate3x3A(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiDilate3x3_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion, not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Erode3x3A(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MorphologyFilter2D.nppiErode3x3_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DilateBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.DilationWithBorderControl.nppiDilateBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilateBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ErodeBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ErosionWithBorderControl.nppiErodeBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErodeBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 dilation with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Dilate3x3Border(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.Dilate3x3Border.nppiDilate3x3Border_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3Border_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Erode3x3Border(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.Erode3x3Border.nppiErode3x3Border_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3Border_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Dilation computes the output pixel as the maximum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void DilateBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.DilationWithBorderControl.nppiDilateBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilateBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Erosion computes the output pixel as the minimum pixel value of the pixels under the mask. Pixels who’s
        /// corresponding mask values are zero to not participate in the maximum search. With border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Mask">Pointer to the start address of the mask array.</param>
        /// <param name="aMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ErodeBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<byte> Mask, NppiSize aMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ErosionWithBorderControl.nppiErodeBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Mask.DevicePointer, aMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErodeBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 dilation with border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Dilate3x3BorderA(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.Dilate3x3Border.nppiDilate3x3Border_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiDilate3x3Border_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 3x3 erosion with border control, ignoring alpha-channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void Erode3x3BorderA(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.Erode3x3Border.nppiErode3x3Border_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiErode3x3Border_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter
        /// <summary>
        /// Pixels under the mask are multiplied by the respective weights in the mask and the results are summed.<para/>
        /// Before writing the result pixel the sum is scaled back via division by nDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="aKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterA(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, NppiSize aKernelSize, NppiPoint oAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, aKernelSize, oAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply convolution filter with user specified 1D column of weights. Result pixel is equal to the sum of
        /// the products between the kernel coefficients (pKernel array) and corresponding neighboring column pixel
        /// values in the source image defined by nKernelDim and nAnchorY, divided by nDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnA(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumn_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowA(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRow_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Apply general linear Row convolution filter, with rescaling, in a 1D mask region around each source pixel with border control. 
        /// Result pixel is equal to the sum of the products between the kernel
        /// coefficients (pKernel array) and corresponding neighboring row pixel values in the source image defined
        /// by iKernelDim and iAnchorX, divided by iDivisor. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nKernelSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nKernelSize, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRowBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, nKernelSize, nAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBoxA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFixedFilters2D.nppiFilterBox_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBox_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMinA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMin_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMin_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region. Not affecting Alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMaxA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMax_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, oMaskSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMax_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 1D column convolution. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnA(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumn32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumn32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 1D row convolution. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. pKernel.Sizes gives kernel size<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowA(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRow32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, pKernel.Size, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRow32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterA(NPPImage_8uC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// convolution filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array.<para/>
        /// Coefficients are expected to be stored in reverse order.</param>
        /// <param name="oKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterA(NPPImage_16sC4 dst, CudaDeviceVariable<float> pKernel, NppiSize oKernelSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.Convolution.nppiFilter32f_8u16s_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pKernel.DevicePointer, oKernelSize, oAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilter32f_8u16s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Prewitt filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittHorizA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittHoriz_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittHoriz_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Prewitt filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittVertA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittVert_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittVert_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Sobel filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SobelHorizA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSobelHoriz_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelHoriz_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Sobel filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelVertA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSobelVert_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelVert_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// horizontal Roberts filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsDownA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsDown_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsDown_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// vertical Roberts filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsUpA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsUp_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsUp_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Laplace filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLaplaceA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLaplace_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLaplace_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Gauss filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterGauss_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGauss_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// High pass filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterHighPassA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterHighPass_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPass_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLowPassA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLowPass_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, eMaskSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPass_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSharpenA(NPPImage_8uC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSharpen_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpen_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region QualityIndex
        /// <summary>
        /// Device scratch buffer size (in bytes) for QualityIndex.
        /// </summary>
        /// <returns></returns>
        public int QualityIndexAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.QualityIndex.nppiQualityIndexGetBufferHostSize_8u32f_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndexGetBufferHostSize_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image QualityIndex. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dst">Pointer to the quality index. (3 * sizeof(float))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="QualityIndexAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void QualityIndexA(NPPImage_8uC4 src2, CudaDeviceVariable<float> dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = QualityIndexAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.QualityIndex.nppiQualityIndex_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, dst.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndex_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image QualityIndex. Not affecting Alpha.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="dst">Pointer to the quality index. (3 * sizeof(float))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void QualityIndexA(NPPImage_8uC4 src2, CudaDeviceVariable<float> dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = QualityIndexAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.QualityIndex.nppiQualityIndex_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, dst.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiQualityIndex_8u32f_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region CountInRange
        /// <summary>
        /// Device scratch buffer size (in bytes) for CountInRange.
        /// </summary>
        /// <returns></returns>
        public int CountInRangeAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.CountInRange.nppiCountInRangeGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCountInRangeGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image CountInRange. Not affecting Alpha.
        /// </summary>
        /// <param name="pCounts">Pointer to the number of pixels that fall into the specified range. (3 * sizeof(int))</param>
        /// <param name="nLowerBound">Fixed size array of the lower bound of the specified range, one per channel.</param>
        /// <param name="nUpperBound">Fixed size array of the upper bound of the specified range, one per channel.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="CountInRangeAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CountInRangeA(CudaDeviceVariable<int> pCounts, byte[] nLowerBound, byte[] nUpperBound, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = CountInRangeAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.CountInRange.nppiCountInRange_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, pCounts.DevicePointer, nLowerBound, nUpperBound, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCountInRange_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image CountInRange. Not affecting Alpha.
        /// </summary>
        /// <param name="pCounts">Pointer to the number of pixels that fall into the specified range. (3 * sizeof(int))</param>
        /// <param name="nLowerBound">Fixed size array of the lower bound of the specified range, one per channel.</param>
        /// <param name="nUpperBound">Fixed size array of the upper bound of the specified range, one per channel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CountInRangeA(CudaDeviceVariable<int> pCounts, byte[] nLowerBound, byte[] nUpperBound, NppStreamContext nppStreamCtx)
        {
            int bufferSize = CountInRangeAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.CountInRange.nppiCountInRange_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, pCounts.DevicePointer, nLowerBound, nUpperBound, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCountInRange_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region GeometryNew

        /// <summary>
        /// image resize. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ResizeSqrPixelA(NPPImage_8uC4 dst, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            NppiRect dstRect = new NppiRect(dst.PointRoi, dst.SizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, srcRect, dst.DevicePointer, dst.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image remap. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RemapA(NPPImage_8uC4 dst, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Remap.nppiRemap_8u_AC4R_Ctx(_devPtr, _sizeOriginal, _pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ScaleA(NPPImage_16sC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u16s_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u16s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// image conversion. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ScaleA(NPPImage_16uC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u16u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u16u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nMin">specifies the minimum saturation value to which every output value will be clamped.</param>
        /// <param name="nMax">specifies the maximum saturation value to which every output value will be clamped.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ScaleA(NPPImage_32fC4 dst, float nMin, float nMax, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nMin, nMax, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image conversion. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void ScaleA(NPPImage_32sC4 dst, NppStreamContext nppStreamCtx)
        {
            NppiRect srcRect = new NppiRect(_pointRoi, _sizeRoi);
            status = NPPNativeMethods_Ctx.NPPi.Scale.nppiScale_8u32s_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiScale_8u32s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region LUT

        /// <summary>
        /// look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTA(NPPImage_8uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUT.nppiLUT_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTCubicA(NPPImage_8uC4 dst, CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTCubic.nppiLUT_Cubic_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// range restricted palette look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points in a palette and 
        /// source pixels are then processed using a restricted bit range when looking up palette values. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pTable">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT palette values.</param>
        /// <param name="nBitSize">Number of least significant bits (must be &gt; 0 and &lt;= 8) of each source pixel value to use as index into palette table during conversion.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTPaletteA(NPPImage_8uC4 dst, CudaDeviceVariable<byte>[] pTable, int nBitSize, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsT = new CUdeviceptr[] { pTable[0].DevicePointer, pTable[1].DevicePointer, pTable[2].DevicePointer };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTPalette.nppiLUTPalette_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, ptrsT, nBitSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUTPalette_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Inplace look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points with no interpolation. Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUT.nppiLUT_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace cubic interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTCubicA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTCubic.nppiLUT_Cubic_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Cubic_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Inplace linear interpolated look-up-table color conversion.
        /// The LUT is derived from a set of user defined mapping points through cubic interpolation.  Not affecting Alpha.
        /// </summary>
        /// <param name="pValues">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined OUTPUT values.</param>
        /// <param name="pLevels">Host pointer to an array of 3 device memory pointers, one per color CHANNEL, pointing to user defined INPUT values. pLevels.Size gives nLevels.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void LUTLinearA(CudaDeviceVariable<int>[] pValues, CudaDeviceVariable<int>[] pLevels, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] ptrsV = new CUdeviceptr[] { pValues[0].DevicePointer, pValues[1].DevicePointer, pValues[2].DevicePointer };
            CUdeviceptr[] ptrsL = new CUdeviceptr[] { pLevels[0].DevicePointer, pLevels[1].DevicePointer, pLevels[2].DevicePointer };
            int[] size = new int[] { pLevels[0].Size, pLevels[1].Size, pLevels[2].Size };
            status = NPPNativeMethods_Ctx.NPPi.ColorLUTLinear.nppiLUT_Linear_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, ptrsV, ptrsL, size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Linear_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MinMaxEveryNew
        /// <summary>
        /// image MinEvery Not affecting Alpha.
        /// </summary>
        /// <param name="src2">Source-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MinEveryA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MinMaxEvery.nppiMinEvery_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMinEvery_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image MaxEvery Not affecting Alpha.
        /// </summary>
        /// <param name="src2">Source-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxEveryA(NPPImage_8uC4 src2, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.MinMaxEvery.nppiMaxEvery_8u_AC4IR_Ctx(src2.DevicePointerRoi, src2.Pitch, _devPtrRoi, _pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaxEvery_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region MirrorNew


        /// <summary>
        /// Mirror image inplace. Not affecting Alpha.
        /// </summary>
        /// <param name="flip">Specifies the axis about which the image is to be mirrored.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MirrorA(NppiAxis flip, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiMirror_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, flip, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMirror_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Copy

        /// <summary>
        /// image copy with nearest source image pixel color. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyReplicateBorderA(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyReplicateBorder.nppiCopyReplicateBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyReplicateBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image copy with the borders wrapped by replication of source image pixel colors. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nTopBorderHeight">Height (in pixels) of the top border. The height of the border at the bottom of
        /// the destination ROI is implicitly defined by the size of the source ROI: nBottomBorderHeight =
        /// oDstSizeROI.height - nTopBorderHeight - oSrcSizeROI.height.</param>
        /// <param name="nLeftBorderWidth">Width (in pixels) of the left border. The width of the border at the right side of
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// the destination ROI is implicitly defined by the size of the source ROI: nRightBorderWidth =
        /// oDstSizeROI.width - nLeftBorderWidth - oSrcSizeROI.width.</param>
        public void CopyWrapBorderA(NPPImage_8uC4 dst, int nTopBorderHeight, int nLeftBorderWidth, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopyWrapBorder.nppiCopyWrapBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nTopBorderHeight, nLeftBorderWidth, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopyWrapBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// linearly interpolated source image subpixel coordinate color copy. Not affecting Alpha.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nDx">Fractional part of source image X coordinate.</param>
        /// <param name="nDy">Fractional part of source image Y coordinate.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CopySubpixA(NPPImage_8uC4 dst, float nDx, float nDy, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.CopySubpix.nppiCopySubpix_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, nDx, nDy, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCopySubpix_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        ///// <summary>
        ///// resizes planar images.
        ///// </summary>
        ///// <param name="src0">Source image (Channel 0)</param>
        ///// <param name="src1">Source image (Channel 1)</param>
        ///// <param name="src2">Source image (Channel 2)</param>
        ///// <param name="src3">Source image (Channel 3)</param>
        ///// <param name="dest0">Destination image (Channel 0)</param>
        ///// <param name="dest1">Destination image (Channel 1)</param>
        ///// <param name="dest2">Destination image (Channel 2)</param>
        ///// <param name="dest3">Destination image (Channel 3)</param>
        ///// <param name="xFactor">X scaling factor</param>
        ///// <param name="yFactor">Y scaling factor</param>
        ///// <param name="eInterpolation">Interpolation mode</param>
        //public static void Resize(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, double xFactor, double yFactor, InterpolationMode eInterpolation)
        //{
        //	CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
        //	CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
        //	NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResize_8u_P4R_Ctx(src, src0.Size, src0.Pitch, new NppiRect(src0.PointRoi, src0.SizeRoi), dst, dest0.Pitch, dest0.SizeRoi, xFactor, yFactor, eInterpolation, nppStreamCtx);
        //	Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResize_8u_P4R_Ctx", status));
        //	NPPException.CheckNppStatus(status, null);
        //}



        /// <summary>
        /// planar image resize.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="nXFactor">Factor by which x dimension is changed. </param>
        /// <param name="nYFactor">Factor by which y dimension is changed. </param>
        /// <param name="nXShift">Source pixel shift in x-direction.</param>
        /// <param name="nYShift">Source pixel shift in y-direction.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ResizeSqrPixel(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, double nXFactor, double nYFactor, double nXShift, double nYShift, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointer, dest1.DevicePointer, dest2.DevicePointer, dest3.DevicePointer };
            NppiRect srcRect = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppiRect dstRect = new NppiRect(dest0.PointRoi, dest0.SizeRoi);
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ResizeSqrPixel.nppiResizeSqrPixel_8u_P4R_Ctx(src, src0.SizeRoi, src0.Pitch, srcRect, dst, dest0.Pitch, dstRect, nXFactor, nYFactor, nXShift, nYShift, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeSqrPixel_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// planar image remap.
        /// </summary>
        /// <param name="src0">Source image (Channel 0)</param>
        /// <param name="src1">Source image (Channel 1)</param>
        /// <param name="src2">Source image (Channel 2)</param>
        /// <param name="src3">Source image (Channel 3)</param>
        /// <param name="dest0">Destination image (Channel 0)</param>
        /// <param name="dest1">Destination image (Channel 1)</param>
        /// <param name="dest2">Destination image (Channel 2)</param>
        /// <param name="dest3">Destination image (Channel 3)</param>
        /// <param name="pXMap">Device memory pointer to 2D image array of X coordinate values to be used when sampling source image. </param>
        /// <param name="pYMap">Device memory pointer to 2D image array of Y coordinate values to be used when sampling source image. </param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void Remap(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3, NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NPPImage_32fC1 pXMap, NPPImage_32fC1 pYMap, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] src = new CUdeviceptr[] { src0.DevicePointer, src1.DevicePointer, src2.DevicePointer, src3.DevicePointer };
            CUdeviceptr[] dst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppiRect srcRect = new NppiRect(src0.PointRoi, src0.SizeRoi);
            NppStatus status = NPPNativeMethods_Ctx.NPPi.Remap.nppiRemap_8u_P4R_Ctx(src, src0.SizeRoi, src0.Pitch, srcRect, pXMap.DevicePointerRoi, pXMap.Pitch, pYMap.DevicePointerRoi, pYMap.Pitch, dst, dest0.Pitch, dest0.SizeRoi, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRemap_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region NormNew
        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_Inf. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffInfAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffInfGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffInfGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_Inf. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffInfAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_InfA(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffInfAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_Inf. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed Inf-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_InfA(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffInfAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_Inf_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L1. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL1AGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffL1GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL1GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L1. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL1AGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L1A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL1AGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L1. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L1-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L1A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL1AGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L1_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormDiff_L2. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormDiffL2AGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiffL2GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiffL2GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormDiff_L2. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormDiffL2AGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L2A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL2AGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormDiff_L2. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormDiff">Pointer to the computed L2-norm of differences. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormDiff_L2A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormDiff, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormDiffL2AGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormDiff.nppiNormDiff_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormDiff.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormDiff_L2_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_Inf. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelInfAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelInfGetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelInfGetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_Inf. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelInfAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_InfA(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelInfAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_Inf. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_InfA(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelInfAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_Inf_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_Inf_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L1. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelL1AGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelL1GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL1GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L1. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL1AGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L1A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL1AGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L1. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L1A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL1AGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L1_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L1_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Device scratch buffer size (in bytes) for NormRel_L2. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int NormRelL2AGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRelL2GetBufferHostSize_8u_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRelL2GetBufferHostSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// image NormRel_L2. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="NormRelL2AGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L2A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL2AGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image NormRel_L2. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="pNormRel">Pointer to the computed relative error for the infinity norm of two images. (3 * sizeof(double))</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void NormRel_L2A(NPPImage_8uC4 tpl, CudaDeviceVariable<double> pNormRel, NppStreamContext nppStreamCtx)
        {
            int bufferSize = NormRelL2AGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.NormRel.nppiNormRel_L2_8u_AC4R_Ctx(_devPtrRoi, _pitch, tpl.DevicePointerRoi, tpl.Pitch, _sizeRoi, pNormRel.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiNormRel_L2_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }





        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_8u32f_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u32f_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_8u32f_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u32f_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_8u32f_AC4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevelA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u32f_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }






        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int FullNormLevelScaledAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiFullNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFullNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="FullNormLevelScaledAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrFull_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FullNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_NormLevel_8u_AC4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int SameNormLevelScaledAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSameNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSameNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="SameNormLevelScaledAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrSame_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = SameNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_NormLevel_8u_AC4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// Device scratch buffer size (in bytes) for CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <returns></returns>
        public int ValidNormLevelScaledAGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiValidNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiValidNormLevelGetBufferHostSize_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="buffer">Allocated device memory with size of at <see cref="ValidNormLevelScaledAGetBufferHostSize()"/></param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// CrossCorrValid_NormLevel. Buffer is internally allocated and freed. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormLevelA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = ValidNormLevelScaledAGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);

            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_NormLevel_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointer, dst.Pitch, nScaleFactor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_NormLevel_8u_AC4RSfs_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }






        /// <summary>
        /// image SqrDistanceFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceFull_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceSame_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceValid_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }



        /// <summary>
        /// image SqrDistanceFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceFull_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceFull_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceFull_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceSame_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceSame_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceSame_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image SqrDistanceValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SqrDistanceValid_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiSqrDistanceValid_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSqrDistanceValid_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// image CrossCorrFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormA(NPPImage_8uC4 tpl, NPPImage_32fC4 dst, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_Norm_8u32f_AC4R_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_8u32f_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// image CrossCorrFull_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrFull_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrFull_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrFull_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrSame_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrSame_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrSame_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrSame_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// image CrossCorrValid_Norm. Not affecting Alpha.
        /// </summary>
        /// <param name="tpl">template image.</param>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nScaleFactor">Integer Result Scaling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void CrossCorrValid_NormA(NPPImage_8uC4 tpl, NPPImage_8uC4 dst, int nScaleFactor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.ImageProximity.nppiCrossCorrValid_Norm_8u_AC4RSfs_Ctx(_devPtrRoi, _pitch, _sizeRoi, tpl.DevicePointerRoi, tpl.Pitch, tpl.SizeRoi, dst.DevicePointerRoi, dst.Pitch, nScaleFactor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCrossCorrValid_Norm_8u_AC4RSfs_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Color Space conversion new in CUDA 6
        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 4 channel 8-bit unsigned packed YUV color conversion with alpha, not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYUV(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.BGRToYUV.nppiBGRToYUV_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYUV_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 4 channel 8-bit unsigned planar YUV color conversion with alpha.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYUV(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYUV.nppiBGRToYUV_8u_AC4P4R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYUV_8u_AC4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit packed YUV with alpha to 4 channel 8-bit unsigned packed BGR color conversion with alpha, not affecting alpha.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void YUVToBGR(NPPImage_8uC4 dest, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.YUVToBGR.nppiYUVToBGR_8u_AC4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYUVToBGR_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 3 channel 8-bit unsigned planar YCbCr color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr_8u_AC4P3R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }


        /// <summary>
        /// 4 channel 8-bit unsigned packed BGR with alpha to 4 channel 8-bit unsigned planar YCbCr color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="dest3">Destination image channel 3</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void BGRToYCbCr(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.BGRToYCbCr.nppiBGRToYCbCr_8u_AC4P4R_Ctx(_devPtrRoi, _pitch, arrayDest, dest0.Pitch, _sizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiBGRToYCbCr_8u_AC4P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        #endregion

        #region LUT Trilinear
        /// <summary>
        /// Four channel 8-bit unsigned 3D trilinear interpolated look-up-table color conversion, with alpha copy.<para/>
        /// Alpha channel is the last channel and is copied to the destination unmodified.<para/>
        /// The LUT is derived from a set of user defined mapping points through trilinear interpolation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Device pointer to aLevels[2] number of contiguous 2D x,y planes of 4-byte packed RGBX values
        /// containing the user defined base OUTPUT values at that x,y, and z (R,G,B) level location. Each level must contain x * y 4-byte
        /// packed pixel values (4th byte is used for alignement only and is ignored) in row (x) order.</param>
        /// <param name="pLevels0">array, cube edge 0, with user defined INPUT level values.</param>
        /// <param name="pLevels1">array, cube edge 1, with user defined INPUT level values.</param>
        /// <param name="pLevels2">array, cube edge 2, with user defined INPUT level values.</param>
        /// <param name="aLevels">Host pointer to an array of 3 user defined number of input/output mapping points, one per 3D cube edge.
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// aLevels[0] represents the number of x axis levels (Red), aLevels[1] represents the number of y axis levels (Green), 
        /// and aLevels[2] represets the number of z axis levels (Blue).</param>
        public void LUTTrilinear(NPPImage_8uC4 dst, CudaDeviceVariable<int> pValues, int[] pLevels0, int[] pLevels1, int[] pLevels2, int[] aLevels, NppStreamContext nppStreamCtx)
        {
            GCHandle h0 = new GCHandle(), h1 = new GCHandle(), h2 = new GCHandle();
            try
            {
                h0 = GCHandle.Alloc(pLevels0, GCHandleType.Pinned);
                h1 = GCHandle.Alloc(pLevels1, GCHandleType.Pinned);
                h2 = GCHandle.Alloc(pLevels2, GCHandleType.Pinned);
                IntPtr[] ptrsLevels = new IntPtr[3];
                ptrsLevels[0] = h0.AddrOfPinnedObject();
                ptrsLevels[1] = h1.AddrOfPinnedObject();
                ptrsLevels[2] = h2.AddrOfPinnedObject();

                status = NPPNativeMethods_Ctx.NPPi.ColorLUTTrilinear.nppiLUT_Trilinear_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pValues.DevicePointer, ptrsLevels, aLevels, nppStreamCtx);
                Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Trilinear_8u_C4R_Ctx", status));
                NPPException.CheckNppStatus(status, this);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h0.Free();
                h1.Free();
                h2.Free();
            }

        }


        /// <summary>
        /// Four channel 8-bit unsigned 3D trilinear interpolated look-up-table color conversion, not affecting alpha.<para/>
        /// Alpha channel is the last channel and is not processed.<para/>
        /// The LUT is derived from a set of user defined mapping points through trilinear interpolation.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="pValues">Device pointer to aLevels[2] number of contiguous 2D x,y planes of 4-byte packed RGBX values
        /// containing the user defined base OUTPUT values at that x,y, and z (R,G,B) level location. Each level must contain x * y 4-byte
        /// packed pixel values (4th byte is used for alignement only and is ignored) in row (x) order.</param>
        /// <param name="pLevels0">array, cube edge 0, with user defined INPUT level values.</param>
        /// <param name="pLevels1">array, cube edge 1, with user defined INPUT level values.</param>
        /// <param name="pLevels2">array, cube edge 2, with user defined INPUT level values.</param>
        /// <param name="aLevels">Host pointer to an array of 3 user defined number of input/output mapping points, one per 3D cube edge.
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// aLevels[0] represents the number of x axis levels (Red), aLevels[1] represents the number of y axis levels (Green), 
        /// and aLevels[2] represets the number of z axis levels (Blue).</param>
        public void LUTTrilinearA(NPPImage_8uC4 dst, CudaDeviceVariable<int> pValues, int[] pLevels0, int[] pLevels1, int[] pLevels2, int[] aLevels, NppStreamContext nppStreamCtx)
        {
            GCHandle h0 = new GCHandle(), h1 = new GCHandle(), h2 = new GCHandle();
            try
            {
                h0 = GCHandle.Alloc(pLevels0, GCHandleType.Pinned);
                h1 = GCHandle.Alloc(pLevels1, GCHandleType.Pinned);
                h2 = GCHandle.Alloc(pLevels2, GCHandleType.Pinned);
                IntPtr[] ptrsLevels = new IntPtr[3];
                ptrsLevels[0] = h0.AddrOfPinnedObject();
                ptrsLevels[1] = h1.AddrOfPinnedObject();
                ptrsLevels[2] = h2.AddrOfPinnedObject();

                status = NPPNativeMethods_Ctx.NPPi.ColorLUTTrilinear.nppiLUT_Trilinear_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, pValues.DevicePointer, ptrsLevels, aLevels, nppStreamCtx);
                Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Trilinear_8u_AC4R_Ctx", status));
                NPPException.CheckNppStatus(status, this);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h0.Free();
                h1.Free();
                h2.Free();
            }

        }


        /// <summary>
        /// Four channel 8-bit unsigned 3D trilinear interpolated look-up-table color conversion, not affecting alpha.<para/>
        /// Alpha channel is the last channel and is not processed.<para/>
        /// The LUT is derived from a set of user defined mapping points through trilinear interpolation.
        /// </summary>
        /// <param name="pValues">Device pointer to aLevels[2] number of contiguous 2D x,y planes of 4-byte packed RGBX values
        /// containing the user defined base OUTPUT values at that x,y, and z (R,G,B) level location. Each level must contain x * y 4-byte
        /// packed pixel values (4th byte is used for alignement only and is ignored) in row (x) order.</param>
        /// <param name="pLevels0">array, cube edge 0, with user defined INPUT level values.</param>
        /// <param name="pLevels1">array, cube edge 1, with user defined INPUT level values.</param>
        /// <param name="pLevels2">array, cube edge 2, with user defined INPUT level values.</param>
        /// <param name="aLevels">Host pointer to an array of 3 user defined number of input/output mapping points, one per 3D cube edge.
        /// <param name="nppStreamCtx">NPP stream context.</param>
        /// aLevels[0] represents the number of x axis levels (Red), aLevels[1] represents the number of y axis levels (Green), 
        /// and aLevels[2] represets the number of z axis levels (Blue).</param>
        public void LUTTrilinearA(CudaDeviceVariable<int> pValues, int[] pLevels0, int[] pLevels1, int[] pLevels2, int[] aLevels, NppStreamContext nppStreamCtx)
        {
            GCHandle h0 = new GCHandle(), h1 = new GCHandle(), h2 = new GCHandle();
            try
            {
                h0 = GCHandle.Alloc(pLevels0, GCHandleType.Pinned);
                h1 = GCHandle.Alloc(pLevels1, GCHandleType.Pinned);
                h2 = GCHandle.Alloc(pLevels2, GCHandleType.Pinned);
                IntPtr[] ptrsLevels = new IntPtr[3];
                ptrsLevels[0] = h0.AddrOfPinnedObject();
                ptrsLevels[1] = h1.AddrOfPinnedObject();
                ptrsLevels[2] = h2.AddrOfPinnedObject();

                status = NPPNativeMethods_Ctx.NPPi.ColorLUTTrilinear.nppiLUT_Trilinear_8u_AC4IR_Ctx(_devPtrRoi, _pitch, _sizeRoi, pValues.DevicePointer, ptrsLevels, aLevels, nppStreamCtx);
                Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiLUT_Trilinear_8u_AC4IR_Ctx", status));
                NPPException.CheckNppStatus(status, this);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                h0.Free();
                h1.Free();
                h2.Free();
            }

        }
        #endregion

        #region SumWindow
        /// <summary>
        /// 8-bit unsigned 1D (column) sum to 32f.
        /// Apply Column Window Summation filter over a 1D mask region around each
        /// source pixel for 4-channel 8 bit/pixel input images with 32-bit floating point
        /// output.  <para/>
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring column pixel values in a mask region of the source image defined by
        /// nMaskSize and nAnchor. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumWindowColumn(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.WindowSum1D.nppiSumWindowColumn_8u32f_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nMaskSize, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowColumn_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// 8-bit unsigned 1D (row) sum to 32f.<para/>
        /// Apply Row Window Summation filter over a 1D mask region around each source
        /// pixel for 4-channel 8-bit pixel input images with 32-bit floating point output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring row pixel values in a mask region of the source image defined
        /// by nKernelDim and nAnchorX. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumWindowRow(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.WindowSum1D.nppiSumWindowRow_8u32f_C4R_Ctx(_devPtrRoi, _pitch, dest.DevicePointerRoi, dest.Pitch, _sizeRoi, nMaskSize, nAnchor, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowRow_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterBorder
        /// <summary>
        /// Four channel 8-bit unsigned convolution filter with border control.<para/>
        /// General purpose 2D convolution filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided.
        /// If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorder(NPPImage_8uC4 dest, CudaDeviceVariable<int> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder.nppiFilterBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 8-bit unsigned convolution filter with border control.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorder(NPPImage_8uC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder32f.nppiFilterBorder32f_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 8-bit unsigned to 16-bit signed convolution filter with border control.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorder(NPPImage_16sC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder32f.nppiFilterBorder32f_8u16s_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_8u16s_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Four channel 8-bit unsigned convolution filter with border control, ignoring alpha channel.<para/>
        /// General purpose 2D convolution filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided.
        /// If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<int> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder.nppiFilterBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 8-bit unsigned convolution filter with border control, ignoring alpha channel.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder32f.nppiFilterBorder32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Four channel 8-bit unsigned to 16-bit signed convolution filter with border control, ignoring alpha channel.<para/>
        /// General purpose 2D convolution filter using floating-point weights with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image. <para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pKernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order</param>
        /// <param name="nKernelSize">Width and Height of the rectangular kernel.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBorderA(NPPImage_16sC4 dest, CudaDeviceVariable<float> pKernel, NppiSize nKernelSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterBorder32f.nppiFilterBorder32f_8u16s_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pKernel.DevicePointer, nKernelSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBorder32f_8u16s_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterSobelBorder
        /// <summary>
        /// Filters the image using a horizontal Sobel filter kernel with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelHorizBorder(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterSobelHorizBorder.nppiFilterSobelHorizBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelHorizBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Filters the image using a vertical Sobel filter kernel with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelVertBorder(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterSobelVertBorder.nppiFilterSobelVertBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelVertBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Filters the image using a horizontal Sobel filter kernel with border control, ignoring alpha channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelHorizBorderA(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterSobelHorizBorder.nppiFilterSobelHorizBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelHorizBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Filters the image using a vertical Sobel filter kernel with border control, ignoring alpha channel.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSobelVertBorderA(NPPImage_8uC4 dest, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterSobelVertBorder.nppiFilterSobelVertBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSobelVertBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region Filter Median
        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMedian(NPPImage_8uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FilterMedianGetBufferHostSize(oMaskSize, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedian_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the Median operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMedian(NPPImage_8uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FilterMedianGetBufferHostSize(oMaskSize, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedian_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for FilterMedian.
        /// </summary>
        /// <returns></returns>
        public int FilterMedianGetBufferHostSize(NppiSize oMaskSize, NppStreamContext nppStreamCtx)
        {
            uint bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedianGetBufferSize_8u_C4R_Ctx(_sizeRoi, oMaskSize, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedianGetBufferSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return (int)bufferSize; //We stay consistent with other GetBufferHostSize functions and convert to int.
        }
        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region, ignoring alpha channel.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMedianA(NPPImage_8uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FilterMedianGetBufferHostSizeA(oMaskSize, nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedian_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_8u_AC4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the median of pixel values under the rectangular mask region, ignoring alpha channel.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Median operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference relative to the source pixel.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the Median operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMedianA(NPPImage_8uC4 dst, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = FilterMedianGetBufferHostSizeA(oMaskSize, nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedian_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, oMaskSize, oAnchor, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedian_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for FilterMedian, ignoring alpha channel.
        /// </summary>
        /// <returns></returns>
        public int FilterMedianGetBufferHostSizeA(NppiSize oMaskSize, NppStreamContext nppStreamCtx)
        {
            uint bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.ImageMedianFilter.nppiFilterMedianGetBufferSize_8u_AC4R_Ctx(_sizeRoi, oMaskSize, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMedianGetBufferSize_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return (int)bufferSize; //We stay consistent with other GetBufferHostSize functions and convert to int.
        }
        #endregion

        #region MaxError
        /// <summary>
        /// image maximum error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxErrorGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.MaximumError.nppiMaximumError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaxError operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaxError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaxErrorGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MaximumError.nppiMaximumError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumError_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaxError.
        /// </summary>
        /// <returns></returns>
        public int MaxErrorGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MaximumError.nppiMaximumErrorGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumErrorGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region AverageError
        /// <summary>
        /// image average error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AverageError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, NppStreamContext nppStreamCtx)
        {
            int bufferSize = AverageErrorGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.AverageError.nppiAverageError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageError operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AverageError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = AverageErrorGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.AverageError.nppiAverageError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageError_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageError.
        /// </summary>
        /// <returns></returns>
        public int AverageErrorGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.AverageError.nppiAverageErrorGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageErrorGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region MaximumRelative_Error
        /// <summary>
        /// image maximum relative error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaximumRelativeError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.MaximumRelativeError.nppiMaximumRelativeError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image maximum relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the MaximumRelativeError operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MaximumRelativeError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = MaximumRelativeErrorGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.MaximumRelativeError.nppiMaximumRelativeError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeError_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for MaximumRelativeError.
        /// </summary>
        /// <returns></returns>
        public int MaximumRelativeErrorGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.MaximumRelativeError.nppiMaximumRelativeErrorGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMaximumRelativeErrorGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion

        #region AverageRelative_Error
        /// <summary>
        /// image average relative error. User buffer is internally allocated and freed.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AverageRelativeError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, NppStreamContext nppStreamCtx)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize(nppStreamCtx);
            CudaDeviceVariable<byte> buffer = new CudaDeviceVariable<byte>(bufferSize);
            status = NPPNativeMethods_Ctx.NPPi.AverageRelativeError.nppiAverageRelativeError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_8u_C4R_Ctx", status));
            buffer.Dispose();
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// image average relative error.
        /// </summary>
        /// <param name="src2">2nd source image</param>
        /// <param name="pError">Pointer to the computed error.</param>
        /// <param name="buffer">Pointer to the user-allocated scratch buffer required for the AverageRelativeError operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void AverageRelativeError(NPPImage_8uC4 src2, CudaDeviceVariable<double> pError, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            int bufferSize = AverageRelativeErrorGetBufferHostSize(nppStreamCtx);
            if (bufferSize > buffer.Size) throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.AverageRelativeError.nppiAverageRelativeError_8u_C4R_Ctx(_devPtrRoi, _pitch, src2.DevicePointerRoi, src2.Pitch, _sizeRoi, pError.DevicePointer, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeError_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Device scratch buffer size (in bytes) for AverageRelativeError.
        /// </summary>
        /// <returns></returns>
        public int AverageRelativeErrorGetBufferHostSize(NppStreamContext nppStreamCtx)
        {
            int bufferSize = 0;
            status = NPPNativeMethods_Ctx.NPPi.AverageRelativeError.nppiAverageRelativeErrorGetBufferHostSize_8u_C4R_Ctx(_sizeRoi, ref bufferSize, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiAverageRelativeErrorGetBufferHostSize_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
            return bufferSize;
        }
        #endregion


        #region FilterGauss
        /// <summary>Filters the image using a Gaussian filter kernel with border control:<para/>
        /// 1/16 2/16 1/16<para/>
        /// 2/16 4/16 2/16<para/>
        /// 1/16 2/16 1/16<para/>
        /// <para/> or <para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 12/571 52/571 127/571 52/571 12/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussBorder(NPPImage_8uC4 dest, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterGaussBorder.nppiFilterGaussBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>Filters the image using a Gaussian filter kernel with border control, ignoring alpha channel:<para/>
        /// 1/16 2/16 1/16<para/>
        /// 2/16 4/16 2/16<para/>
        /// 1/16 2/16 1/16<para/>
        /// <para/> or <para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 12/571 52/571 127/571 52/571 12/571<para/>
        /// 7/571 31/571 52/571 31/571 7/571<para/>
        /// 2/571 7/571 12/571 7/571 2/571<para/>
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussBorderA(NPPImage_8uC4 dest, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterGaussBorder.nppiFilterGaussBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion



        //New in Cuda 7.0

        #region FilterColumnBorder
        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnBorder(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumnBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnBorder(NPPImage_8uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumnBorder32f_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. Before writing the result pixel the sum is scaled
        /// back via division by nDivisor. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="nDivisor">The factor by which the convolved summation from the Filter operation should be divided. If equal to the sum of coefficients, this will keep the maximum result value within full scale.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<int> Kernel, int nAnchor, int nDivisor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumnBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, nDivisor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution column filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterColumnBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterColumnBorder32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterColumnBorder32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterRow
        /// <summary>
        /// General purpose 1D convolution row filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowBorder(NPPImage_8uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRowBorder32f_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// General purpose 1D convolution row filter with border control.<para/>
        /// Pixels under the mask are multiplied by the respective weights in the mask
        /// and the results are summed. If any portion of the mask overlaps the source
        /// image boundary the requested border type operation is applied to all mask pixels
        /// which fall outside of the source image.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="Kernel">Pointer to the start address of the kernel coefficient array. Coeffcients are expected to be stored in reverse order.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRowBorderA(NPPImage_8uC4 dest, CudaDeviceVariable<float> Kernel, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFilter1D.nppiFilterRowBorder32f_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, Kernel.DevicePointer, Kernel.Size, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRowBorder32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        #endregion

        #region SumWindow

        /// <summary>
        /// Apply Column Window Summation filter over a 1D mask region around each
        /// source pixel for 3-channel 8 bit/pixel input images with 32-bit floating point
        /// output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring column pixel values in a mask region of the source image defined by
        /// nMaskSize and nAnchor. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">Y offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumWindowColumnBorder(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.WindowSum1D.nppiSumWindowColumnBorder_8u32f_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nMaskSize, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowColumnBorder_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Apply Row Window Summation filter over a 1D mask region around each source
        /// pixel for 3-channel 8-bit pixel input images with 32-bit floating point output.  
        /// Result 32-bit floating point pixel is equal to the sum of the corresponding and
        /// neighboring row pixel values in a mask region of the source image defined
        /// by nKernelDim and nAnchorX. 
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="nMaskSize">Length of the linear kernel array.</param>
        /// <param name="nAnchor">X offset of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void SumWindowRowBorder(NPPImage_32fC4 dest, int nMaskSize, int nAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.WindowSum1D.nppiSumWindowRowBorder_8u32f_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nMaskSize, nAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiSumWindowRowBorder_8u32f_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterBox


        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBoxBorder(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFixedFilters2D.nppiFilterBoxBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBoxBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Computes the average pixel values of the pixels under a rectangular mask.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterBoxBorderA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.LinearFixedFilters2D.nppiFilterBoxBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterBoxBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Filter Min/Max


        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMinBorder(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMinBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMinBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMaxBorder(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMaxBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMaxBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// Result pixel value is the minimum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMinBorderA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMinBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMinBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Result pixel value is the maximum of pixel values under the rectangular mask region.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="oMaskSize">Width and Height of the neighborhood region for the local Avg operation.</param>
        /// <param name="oAnchor">X and Y offsets of the kernel origin frame of reference w.r.t the source pixel.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterMaxBorderA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.RankFilters.nppiFilterMaxBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterMaxBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion

        #region FilterOthers


        /// <summary>
        /// horizontal Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittHorizBorder(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittHorizBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittHorizBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittHorizBorderA(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittHorizBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittHorizBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittVertBorder(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittVertBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittVertBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// vertical Prewitt filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterPrewittVertBorderA(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterPrewittVertBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterPrewittVertBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Roberts filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsDownBorder(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsDownBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsDownBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// horizontal Roberts filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsDownBorderA(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsDownBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsDownBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Roberts filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsUpBorder(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsUpBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsUpBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// vertical Roberts filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterRobertsUpBorderA(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterRobertsUpBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterRobertsUpBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Laplace filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLaplaceBorder(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLaplaceBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLaplaceBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Laplace filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLaplaceBorderA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLaplaceBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLaplaceBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterHighPassBorder(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterHighPassBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPassBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// High pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterHighPassBorderA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterHighPassBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterHighPassBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLowPassBorder(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLowPassBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPassBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Low pass filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eMaskSize">Enumeration value specifying the mask size.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterLowPassBorderA(NPPImage_8uC4 dst, MaskSize eMaskSize, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterLowPassBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eMaskSize, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterLowPassBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSharpenBorder(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSharpenBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpenBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        /// <summary>
        /// Sharpen filter.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterSharpenBorderA(NPPImage_8uC4 dst, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterSharpenBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterSharpenBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion
        #region Filter Unsharp

        /// <summary>
        /// Filters the image using a unsharp-mask sharpening filter kernel with border control.<para/>
        /// The algorithm involves the following steps:<para/>
        /// Smooth the original image with a Gaussian filter, with the width controlled by the nRadius.<para/>
        /// Subtract the smoothed image from the original to create a high-pass filtered image.<para/>
        /// Apply any clipping needed on the high-pass image, as controlled by the nThreshold.<para/>
        /// Add a certain percentage of the high-pass filtered image to the original image, 
        /// with the percentage controlled by the nWeight.
        /// In pseudocode this algorithm can be written as:<para/>
        /// HighPass = Image - Gaussian(Image)<para/>
        /// Result = Image + nWeight * HighPass * ( |HighPass| >= nThreshold ) <para/>
        /// where nWeight is the amount, nThreshold is the threshold, and >= indicates a Boolean operation, 1 if true, or 0 otherwise.
        /// <para/>
        /// If any portion of the mask overlaps the source image boundary, the requested border type 
        /// operation is applied to all mask pixels which fall outside of the source image.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <param name="nWeight">The percentage of the difference between the original and the high pass image that is added back into the original.</param>
        /// <param name="nThreshold">The threshold needed to apply the difference amount.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="buffer">Pointer to the user-allocated device scratch buffer required for the unsharp operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterUnsharpBorder(NPPImage_8uC4 dst, float nRadius, float nSigma, float nWeight, float nThreshold, NppiBorderType eBorderType, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            if (buffer.Size < FilterUnsharpGetBufferSize(nRadius, nSigma))
                throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterUnsharpBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, new NppiPoint(), dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nRadius, nSigma, nWeight, nThreshold, eBorderType, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a unsharp-mask sharpening filter kernel with border control.<para/>
        /// The algorithm involves the following steps:<para/>
        /// Smooth the original image with a Gaussian filter, with the width controlled by the nRadius.<para/>
        /// Subtract the smoothed image from the original to create a high-pass filtered image.<para/>
        /// Apply any clipping needed on the high-pass image, as controlled by the nThreshold.<para/>
        /// Add a certain percentage of the high-pass filtered image to the original image, 
        /// with the percentage controlled by the nWeight.
        /// In pseudocode this algorithm can be written as:<para/>
        /// HighPass = Image - Gaussian(Image)<para/>
        /// Result = Image + nWeight * HighPass * ( |HighPass| >= nThreshold ) <para/>
        /// where nWeight is the amount, nThreshold is the threshold, and >= indicates a Boolean operation, 1 if true, or 0 otherwise.
        /// <para/>
        /// If any portion of the mask overlaps the source image boundary, the requested border type 
        /// operation is applied to all mask pixels which fall outside of the source image.
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="nRadius">The radius of the Gaussian filter, in pixles, not counting the center pixel.</param>
        /// <param name="nSigma">The standard deviation of the Gaussian filter, in pixel.</param>
        /// <param name="nWeight">The percentage of the difference between the original and the high pass image that is added back into the original.</param>
        /// <param name="nThreshold">The threshold needed to apply the difference amount.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="buffer">Pointer to the user-allocated device scratch buffer required for the unsharp operation.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterUnsharpBorderA(NPPImage_8uC4 dst, float nRadius, float nSigma, float nWeight, float nThreshold, NppiBorderType eBorderType, CudaDeviceVariable<byte> buffer, NppStreamContext nppStreamCtx)
        {
            if (buffer.Size < FilterUnsharpGetBufferSizeA(nRadius, nSigma))
                throw new NPPException("Provided buffer is too small.");

            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterUnsharpBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, new NppiPoint(), dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, nRadius, nSigma, nWeight, nThreshold, eBorderType, buffer.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterUnsharpBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        #region Filter Gauss Advanced

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGauss(NPPImage_8uC4 dst, CudaDeviceVariable<float> Kernel, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterGaussAdvanced_8u_C4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, Kernel.Size, Kernel.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvanced_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussBorder(NPPImage_8uC4 dst, CudaDeviceVariable<float> Kernel, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterGaussBorder.nppiFilterGaussAdvancedBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, Kernel.Size, Kernel.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvancedBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussA(NPPImage_8uC4 dst, CudaDeviceVariable<float> Kernel, NppStreamContext nppStreamCtx)
        {
            status = NPPNativeMethods_Ctx.NPPi.FixedFilters.nppiFilterGaussAdvanced_8u_AC4R_Ctx(_devPtrRoi, _pitch, dst.DevicePointerRoi, dst.Pitch, _sizeRoi, Kernel.Size, Kernel.DevicePointer, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvanced_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Filters the image using a separable Gaussian filter kernel with user supplied floating point coefficients
        /// </summary>
        /// <param name="dst">Destination-Image</param>
        /// <param name="Kernel">Pointer to an array of nFilterTaps kernel coefficients which sum to 1.0F, where nFilterTaps =  2 * ((int)((float)ceil(radius) + 0.5F) ) + 1.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterGaussBorderA(NPPImage_8uC4 dst, CudaDeviceVariable<float> Kernel, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterGaussBorder.nppiFilterGaussAdvancedBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dst.DevicePointerRoi, dst.Pitch, dst.SizeRoi, Kernel.Size, Kernel.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterGaussAdvancedBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        #endregion

        //New in Cuda 8.0
        #region ColorConversion new in Cuda 8.0


        /// <summary>
        /// 4 channel 8-bit unsigned packed RGB with alpha to 3 channel 8-bit unsigned planar YCbCr411 color conversion.
        /// </summary>
        /// <param name="dest0">Destination image channel 0</param>
        /// <param name="dest1">Destination image channel 1</param>
        /// <param name="dest2">Destination image channel 2</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void RGBToYCbCr411(NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arrayDest = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            int[] arrayPitch = new int[] { dest0.Pitch, dest1.Pitch, dest2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.RGBToYCbCr.nppiRGBToYCbCr411_8u_AC4P3R_Ctx(DevicePointerRoi, Pitch, arrayDest, arrayPitch, SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiRGBToYCbCr411_8u_AC4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 3 channel 8-bit unsigned planar YCbCr411 to 4 channel 8-bit unsigned packed RGB color conversion with constant alpha.
        /// </summary>
        /// <param name="src0">Source image channel 0</param>
        /// <param name="src1">Source image channel 1</param>
        /// <param name="src2">Source image channel 2</param>
        /// <param name="dest">Destination image</param>
        /// <param name="nAval">8-bit unsigned alpha constant.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void YCbCr411ToRGB(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC4 dest, byte nAval, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi };
            int[] arrayPitch = new int[] { src0.Pitch, src1.Pitch, src2.Pitch };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.YCbCrToRGB.nppiYCbCr411ToRGB_8u_P3C4R_Ctx(arraySrc, arrayPitch, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, nAval, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYCbCr411ToRGB_8u_P3C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        //New in Cuda 9.0/9.1
        #region New Cuda9





        /// <summary>
        /// Wiener filter with border control.
        /// </summary>
        /// <param name="dest">destination_image_pointer</param>
        /// <param name="oMaskSize">Pixel Width and Height of the rectangular region of interest surrounding the source pixel.</param>
        /// <param name="oAnchor">Positive X and Y relative offsets of primary pixel in region of interest surrounding the source pixel relative to bottom right of oMaskSize.</param>
        /// <param name="aNoise">Fixed size array of per-channel noise variance level value in range of 0.0F to 1.0F.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterWienerBorder(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, float[] aNoise, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterWienerBorder.nppiFilterWienerBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, aNoise, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterWienerBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// Wiener filter with border control, ignoring alpha channel.
        /// </summary>
        /// <param name="dest">destination_image_pointer</param>
        /// <param name="oMaskSize">Pixel Width and Height of the rectangular region of interest surrounding the source pixel.</param>
        /// <param name="oAnchor">Positive X and Y relative offsets of primary pixel in region of interest surrounding the source pixel relative to bottom right of oMaskSize.</param>
        /// <param name="aNoise">Fixed size array of per-channel noise variance level value in range of 0.0F to 1.0F.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void FilterWienerBorderA(NPPImage_8uC4 dest, NppiSize oMaskSize, NppiPoint oAnchor, float[] aNoise, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.FilterWienerBorder.nppiFilterWienerBorder_8u_AC4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, oMaskSize, oAnchor, aNoise, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiFilterWienerBorder_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }




        /// <summary>
        /// 4 channel 8-bit unsigned integer morphological close with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pMask">Pointer to the start address of the mask array</param>
        /// <param name="oMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="pBuffer">Pointer to device memory scratch buffer at least as large as value returned by the corresponding MorphGetBufferSize call.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MorphCloseBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> pMask, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> pBuffer, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ComplexImageMorphology.nppiMorphCloseBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pMask.DevicePointer, oMaskSize, oAnchor, pBuffer.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMorphCloseBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }


        /// <summary>
        /// 4 channel 8-bit unsigned integer morphological open with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pMask">Pointer to the start address of the mask array</param>
        /// <param name="oMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="pBuffer">Pointer to device memory scratch buffer at least as large as value returned by the corresponding MorphGetBufferSize call.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MorphOpenBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> pMask, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> pBuffer, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ComplexImageMorphology.nppiMorphOpenBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pMask.DevicePointer, oMaskSize, oAnchor, pBuffer.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMorphOpenBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned integer morphological top hat with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pMask">Pointer to the start address of the mask array</param>
        /// <param name="oMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="pBuffer">Pointer to device memory scratch buffer at least as large as value returned by the corresponding MorphGetBufferSize call.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MorphTopHatBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> pMask, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> pBuffer, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ComplexImageMorphology.nppiMorphTopHatBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pMask.DevicePointer, oMaskSize, oAnchor, pBuffer.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMorphTopHatBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned integer morphological black hat with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pMask">Pointer to the start address of the mask array</param>
        /// <param name="oMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="pBuffer">Pointer to device memory scratch buffer at least as large as value returned by the corresponding MorphGetBufferSize call.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MorphBlackHatBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> pMask, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> pBuffer, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ComplexImageMorphology.nppiMorphBlackHatBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pMask.DevicePointer, oMaskSize, oAnchor, pBuffer.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMorphBlackHatBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned integer morphological gradient with border control.
        /// </summary>
        /// <param name="dest">Destination image</param>
        /// <param name="pMask">Pointer to the start address of the mask array</param>
        /// <param name="oMaskSize">Width and Height mask array.</param>
        /// <param name="oAnchor">X and Y offsets of the mask origin frame of reference w.r.t the source pixel.</param>
        /// <param name="pBuffer">Pointer to device memory scratch buffer at least as large as value returned by the corresponding MorphGetBufferSize call.</param>
        /// <param name="eBorderType">The border type operation to be applied at source image border boundaries.</param>
        /// <param name="filterArea">The area where the filter is allowed to read pixels. The point is relative to the ROI set to source image, the size is the total size starting from the filterArea point. Default value is the set ROI.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public void MorphGradientBorder(NPPImage_8uC4 dest, CudaDeviceVariable<byte> pMask, NppiSize oMaskSize, NppiPoint oAnchor, CudaDeviceVariable<byte> pBuffer, NppiBorderType eBorderType, NppStreamContext nppStreamCtx, NppiRect filterArea = new NppiRect())
        {
            if (filterArea.Size == new NppiSize())
            {
                filterArea.Size = _sizeRoi;
            }
            status = NPPNativeMethods_Ctx.NPPi.ComplexImageMorphology.nppiMorphGradientBorder_8u_C4R_Ctx(_devPtrRoi, _pitch, filterArea.Size, filterArea.Location, dest.DevicePointerRoi, dest.Pitch, dest.SizeRoi, pMask.DevicePointer, oMaskSize, oAnchor, pBuffer.DevicePointer, eBorderType, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiMorphGradientBorder_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, this);
        }
        #endregion


        #region New in Cuda 10.0

        /// <summary>
        /// image resize batch.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oSmallestDstSize">Size in pixels of the entire smallest destination image width and height, may be from different images.</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiResizeBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ResizeBatch(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiSize oSmallestDstSize, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiResizeBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResizeBatch_8u_C4R_Ctx(oSmallestSrcSize, oSrcRectROI, oSmallestDstSize, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeBatch_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image resize batch not affecting alpha.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oSmallestDstSize">Size in pixels of the entire smallest destination image width and height, may be from different images.</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiResizeBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ResizeBatchA(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiSize oSmallestDstSize, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiResizeBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResizeBatch_8u_AC4R_Ctx(oSmallestSrcSize, oSrcRectROI, oSmallestDstSize, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeBatch_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image resize batch for variable ROI.
        /// </summary>
        /// <param name="nMaxWidth">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="nMaxHeight">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="pBatchSrc">Size in pixels of the entire smallest destination image width and height, may be from different images.</param>
        /// <param name="pBatchDst">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="nBatchSize">Device memory pointer to nBatchSize list of NppiResizeBatchCXR structures.</param>
        /// <param name="pBatchROI">Device pointer to NppiResizeBatchROI_Advanced list of per-image variable ROIs.User needs to initialize this structure and copy it to device.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ResizeBatchAdvanced(int nMaxWidth, int nMaxHeight, CudaDeviceVariable<NppiImageDescriptor> pBatchSrc, CudaDeviceVariable<NppiImageDescriptor> pBatchDst,
                                        CudaDeviceVariable<NppiResizeBatchROI_Advanced> pBatchROI, uint nBatchSize, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResizeBatch_8u_C4R_Advanced_Ctx(nMaxWidth, nMaxHeight, pBatchSrc.DevicePointer, pBatchDst.DevicePointer,
                pBatchROI.DevicePointer, pBatchDst.Size, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeBatch_8u_C4R_Advanced_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image resize batch for variable ROI not affecting alpha.
        /// </summary>
        /// <param name="nMaxWidth">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="nMaxHeight">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="pBatchSrc">Size in pixels of the entire smallest destination image width and height, may be from different images.</param>
        /// <param name="pBatchDst">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="nBatchSize">Device memory pointer to nBatchSize list of NppiResizeBatchCXR structures.</param>
        /// <param name="pBatchROI">Device pointer to NppiResizeBatchROI_Advanced list of per-image variable ROIs.User needs to initialize this structure and copy it to device.</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ResizeBatchAdvancedA(int nMaxWidth, int nMaxHeight, CudaDeviceVariable<NppiImageDescriptor> pBatchSrc, CudaDeviceVariable<NppiImageDescriptor> pBatchDst,
                                        CudaDeviceVariable<NppiResizeBatchROI_Advanced> pBatchROI, uint nBatchSize, InterpolationMode eInterpolation, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiResizeBatch_8u_AC4R_Advanced_Ctx(nMaxWidth, nMaxHeight, pBatchSrc.DevicePointer, pBatchDst.DevicePointer,
                pBatchROI.DevicePointer, pBatchDst.Size, eInterpolation, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiResizeBatch_8u_AC4R_Advanced_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion

        #region New in Cuda 10.2

        /// <summary>
        /// image warp affine batch.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiWarpAffineBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpAffineBatch(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiWarpAffineBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiWarpAffineBatch_8u_C4R_Ctx(oSmallestSrcSize, oSrcRectROI, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBatch_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image warp perspective batch.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiWarpAffineBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpPerspectiveBatch(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiWarpAffineBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiWarpPerspectiveBatch_8u_C4R_Ctx(oSmallestSrcSize, oSrcRectROI, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBatch_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image warp affine batch not affecting alpha.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiWarpAffineBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpAffineBatchA(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiWarpAffineBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiWarpAffineBatch_8u_AC4R_Ctx(oSmallestSrcSize, oSrcRectROI, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpAffineBatch_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// image warp perspective batch not affecting alpha.
        /// </summary>
        /// <param name="oSmallestSrcSize">Size in pixels of the entire smallest source image width and height, may be from different images.</param>
        /// <param name="oSrcRectROI">Region of interest in the source images (may overlap source image size width and height).</param>
        /// <param name="oDstRectROI">Region of interest in the destination images (may overlap destination image size width and height).</param>
        /// <param name="eInterpolation">The type of eInterpolation to perform resampling. Currently limited to NPPI_INTER_NN, NPPI_INTER_LINEAR, NPPI_INTER_CUBIC, or NPPI_INTER_SUPER. </param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiWarpAffineBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void WarpPerspectiveBatchA(NppiSize oSmallestSrcSize, NppiRect oSrcRectROI, NppiRect oDstRectROI, InterpolationMode eInterpolation, CudaDeviceVariable<NppiWarpAffineBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.GeometricTransforms.nppiWarpPerspectiveBatch_8u_AC4R_Ctx(oSmallestSrcSize, oSrcRectROI, oDstRectROI, eInterpolation, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiWarpPerspectiveBatch_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }


        /// <summary>
        /// color twist batch
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within the ROI for each image in batch. Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatch(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32f_8u_C4R_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32f_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }


        /// <summary>
        /// in place color twist batch
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within the ROI for each image in batch. Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatchI(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32f_8u_C4IR_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32f_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }


        /// <summary>
        /// color twist batch not affecting alpha
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within the ROI for each image in batch. Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatchA(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32f_8u_AC4R_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32f_8u_AC4R_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }


        /// <summary>
        /// in place color twist batch not affecting alpha
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within the ROI for each image in batch. Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatchIA(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32f_8u_AC4IR_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32f_8u_AC4IR_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }


        /// <summary>
        /// 4 channel 8-bit unsigned integer color twist with 4x5 matrix including a constant vector (20 coefficients total).
        /// An input 4x5 color twist matrix with floating-point coefficient values including a constant (in the fourth column) vector
        /// is applied within ROI.  For this particular version of the function the result is generated as shown below.
        ///  \code
        ///      dst[0] = aTwist[0][0]		/// src[0] + aTwist[0][1]		/// src[1] + aTwist[0][2]		/// src[2] + aTwist[0][3]		/// src[3] + aTwist[0][4]
        ///      dst[1] = aTwist[1][0]		/// src[0] + aTwist[1][1]		/// src[1] + aTwist[1][2]		/// src[2] + aTwist[1][3]		/// src[3] + aTwist[1][4]
        ///      dst[2] = aTwist[2][0]		/// src[0] + aTwist[2][1]		/// src[1] + aTwist[2][2]		/// src[2] + aTwist[2][3]		/// src[3] + aTwist[2][4]
        ///      dst[3] = aTwist[3][0]		/// src[0] + aTwist[3][1]		/// src[1] + aTwist[3][2]		/// src[2] + aTwist[3][3]		/// src[3] + aTwist[3][4]
        ///  \endcode
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within ROI for each image in batch.  Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatchC(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32fC_8u_C4R_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32fC_8u_C4R_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }


        /// <summary>
        /// 4 channel 8-bit unsigned integer in place color twist with 4x5 matrix including a constant vector (20 coefficients total).
        /// An input 4x5 color twist matrix with floating-point coefficient values including a constant (in the fourth column) vector
        /// is applied within ROI.  For this particular version of the function the result is generated as shown below.
        ///  \code
        ///      dst[0] = aTwist[0][0]		/// src[0] + aTwist[0][1]		/// src[1] + aTwist[0][2]		/// src[2] + aTwist[0][3]		/// src[3] + aTwist[0][4]
        ///      dst[1] = aTwist[1][0]		/// src[0] + aTwist[1][1]		/// src[1] + aTwist[1][2]		/// src[2] + aTwist[1][3]		/// src[3] + aTwist[1][4]
        ///      dst[2] = aTwist[2][0]		/// src[0] + aTwist[2][1]		/// src[1] + aTwist[2][2]		/// src[2] + aTwist[2][3]		/// src[3] + aTwist[2][4]
        ///      dst[3] = aTwist[3][0]		/// src[0] + aTwist[3][1]		/// src[1] + aTwist[3][2]		/// src[2] + aTwist[3][3]		/// src[3] + aTwist[3][4]
        ///  \endcode
        /// An input color twist matrix with floating-point coefficient values is applied
        /// within ROI for each image in batch.  Color twist matrix can vary per image. The same ROI is applied to each image.
        /// </summary>
        /// <param name="nMin">Minimum clamp value.</param>
        /// <param name="nMax">Maximum saturation and clamp value.</param>
        /// <param name="oSizeROI"></param>
        /// <param name="pBatchList">Device memory pointer to nBatchSize list of NppiColorTwistBatchCXR structures.</param>
        /// <param name="nppStreamCtx">NPP stream context.</param>
        public static void ColorTwistBatchIC(float nMin, float nMax, NppiSize oSizeROI, CudaDeviceVariable<NppiColorTwistBatchCXR> pBatchList, NppStreamContext nppStreamCtx)
        {
            NppStatus status = NPPNativeMethods_Ctx.NPPi.ColorTwistBatch.nppiColorTwistBatch32fC_8u_C4IR_Ctx(nMin, nMax, oSizeROI, pBatchList.DevicePointer, pBatchList.Size, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiColorTwistBatch32fC_8u_C4IR_Ctx", status));
            NPPException.CheckNppStatus(status, pBatchList);
        }
        #endregion

        #region New in Cuda 11.1
        /// <summary>
        /// 4 channel 8-bit unsigned planar JPEG YCCK color format to 4 channel 8-bit unsigned planar CMYK color conversion using 601 RGB color coefficients and CMY inversion.
        /// </summary>
        public static void YCCKToCMYK_JPEG_601(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3,
            NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NPPImage_8uC1 dest3, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            CUdeviceptr[] arrayDst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi, dest3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.YCCKToCMYK_JPEG.nppiYCCKToCMYK_JPEG_601_8u_P4R_Ctx(arraySrc, src0.Pitch, arrayDst, dest0.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiYCCKToCMYK_JPEG_601_8u_P4R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned planar JPEG CMYK or YCCK color model image to 3 channel 8-bit unsigned planar RGB color model image without color profile conversion.
        /// </summary>
        public static void CMYKOrYCCKToRGB_JPEG(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3,
            NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            CUdeviceptr[] arrayDst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.CMYK_OR_YCCK_JPEG_TO_RGB.nppiCMYKOrYCCKToRGB_JPEG_8u_P4P3R_Ctx(arraySrc, src0.Pitch, arrayDst, dest0.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCMYKOrYCCKToRGB_JPEG_8u_P4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned planar JPEG CMYK or YCCK color model image to 3 channel 8-bit unsigned packed RGB color model image without color profile conversion
        /// </summary>
        public static void CMYKOrYCCKToRGB_JPEG(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3,
            NPPImage_8uC3 dest, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.CMYK_OR_YCCK_JPEG_TO_RGB.nppiCMYKOrYCCKToRGB_JPEG_8u_P4C3R_Ctx(arraySrc, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCMYKOrYCCKToRGB_JPEG_8u_P4C3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }



        /// <summary>
        /// 4 channel 8-bit unsigned planar JPEG CMYK or YCCK color model image to 3 channel 8-bit unsigned planar BGR color model image without color profile conversion.
        /// </summary>
        public static void CMYKOrYCCKToBGR_JPEG(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3,
            NPPImage_8uC1 dest0, NPPImage_8uC1 dest1, NPPImage_8uC1 dest2, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            CUdeviceptr[] arrayDst = new CUdeviceptr[] { dest0.DevicePointerRoi, dest1.DevicePointerRoi, dest2.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.CMYK_OR_YCCK_JPEG_TO_BGR.nppiCMYKOrYCCKToBGR_JPEG_8u_P4P3R_Ctx(arraySrc, src0.Pitch, arrayDst, dest0.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCMYKOrYCCKToBGR_JPEG_8u_P4P3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }

        /// <summary>
        /// 4 channel 8-bit unsigned planar JPEG CMYK or YCCK color model image to 3 channel 8-bit unsigned packed BGR color model image without color profile conversion.
        /// </summary>
        public static void CMYKOrYCCKToBGR_JPEG(NPPImage_8uC1 src0, NPPImage_8uC1 src1, NPPImage_8uC1 src2, NPPImage_8uC1 src3,
            NPPImage_8uC3 dest, NppStreamContext nppStreamCtx)
        {
            CUdeviceptr[] arraySrc = new CUdeviceptr[] { src0.DevicePointerRoi, src1.DevicePointerRoi, src2.DevicePointerRoi, src3.DevicePointerRoi };
            NppStatus status = NPPNativeMethods_Ctx.NPPi.CMYK_OR_YCCK_JPEG_TO_BGR.nppiCMYKOrYCCKToBGR_JPEG_8u_P4C3R_Ctx(arraySrc, src0.Pitch, dest.DevicePointerRoi, dest.Pitch, src0.SizeRoi, nppStreamCtx);
            Debug.WriteLine(String.Format("{0:G}, {1}: {2}", DateTime.Now, "nppiCMYKOrYCCKToBGR_JPEG_8u_P4C3R_Ctx", status));
            NPPException.CheckNppStatus(status, null);
        }
        #endregion
    }
}
