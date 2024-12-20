import { getProfileDetails, ProfileDetailsData } from "../../api/profileApi";
import React, { useState, useEffect } from "react";
import { CircularProgress, FormControlLabel } from '@mui/material';
import CustomSwitch from "../../CustomSwitch";
interface ProfileCardProps {
  handleClickEditProfileCard : (event: React.MouseEvent<HTMLButtonElement>) => void;
  profileDetails : ProfileDetailsData | null;
  loading: boolean
}

export default function ProfileCard(props: ProfileCardProps)
{
    return(
        <>
            {!props.loading ? <div className="profile-card">
                <img className="animal-image" src={`data:${props.profileDetails?.images[0].imageFormat};base64,${props.profileDetails?.images[0].imageData}`} alt="Animal"/>
                <h1 className="profile-card__name">{props.profileDetails?.profile.name}, {props.profileDetails?.profile.age}</h1>
                <p className="profile-card__title">{props.profileDetails?.animal.type}, {props.profileDetails?.animal.breed}</p>
                <p>{props.profileDetails?.profile.description}</p>
                <FormControlLabel
                  control={
                    <CustomSwitch name="isVaccinated" checked={props.profileDetails?.profile.isVaccinated}/>
                  }
                  label="Vacinated:"
                  labelPlacement="start"
                  sx={{
                    fontFamily: "'Karla', sans-serif", 
                    fontSize: "14px",                 
                    ".MuiFormControlLabel-label": {
                      fontFamily: "'Karla', sans-serif", 
                    },
                  }}
                />
                <FormControlLabel
                  control={
                    <CustomSwitch name="isSterilized" checked={props.profileDetails?.profile.isSterilized}/>
                  }
                  label="Sterialized:"
                  labelPlacement="start"
                  sx={{
                    fontFamily: "'Karla', sans-serif", 
                    fontSize: "14px",                  
                    ".MuiFormControlLabel-label": {
                      fontFamily: "'Karla', sans-serif", 
                    },
                  }}
                />
                <button className="edit-profile-btn" onClick={props.handleClickEditProfileCard}>Edit Profile</button>
            </div> : <CircularProgress sx={{color: "#864c4c" }} size={48} />
        }
        </>
        
    );
}