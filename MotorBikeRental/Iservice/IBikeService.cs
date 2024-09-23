using System;
using MotorBikeRental.DTOs.RequestDTO;
using MotorBikeRental.DTOs.ResponseDTO;

namespace MotorBikeRental.Iservice
{

    public interface IBikeService
{
    Task<List<BikeResponseDTO>> AddBike(BikeRequestDTO bikeRequestDTO);
    Task <List<BikeResponseDTO>> GetAllBikes();
    Task <bool> DeleteBike(int Id);
    Task <List<BikeImageResponseDTO>> AddImages(BikeImageRequestDTO imageRequestDTO);
    Task <bool> UpdateImages(int ImageId,BikeImageRequestDTO imageRequestDTO);
    

}

}


