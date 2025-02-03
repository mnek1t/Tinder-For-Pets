import "../../styles/form.css";
import React, { useState } from "react";
import LocationSuggestion from "./LocationSuggestion"; 
import { ProfileData } from "../../api/profileApi";
import Error from "./Error";
import LoadButton from "../LoadButton";
import { FormGroup, FormControlLabel } from "@mui/material";
import CustomSwitch from "../CustomSwitch";
import FileUploader from "../FileUploader";
import { DEFAULT_SEX_ID, DEFAULT_ANIMAL_TYPE_ID, DEFAULT_BREED_ID } from "../../utils/TinderConstants";
interface CreateProfileProps {
    animalTypes: Array<{ id: number; typeName: string }>;
    sexes: Array<{ id: number; sexName: string }>;
    breeds: Array<{ id: number; breedName: string }>;
    error?: Error | null;
    loading: boolean,
    handleCreateProfile : (petProfileData : ProfileData) => void,
    handleFetchBreeds : (animalTypeId : number) => void
}

export default function CreateProfileForm(props : CreateProfileProps) {
    const [profileData, setProfileData] = useState<ProfileData>({
        name: "",
        description: "",
        typeId: props.animalTypes[0]?.id || DEFAULT_ANIMAL_TYPE_ID,
        breedId: props.breeds[0]?.id || DEFAULT_BREED_ID,
        dateOfBirth: "",
        sexId: props.sexes[0]?.id || DEFAULT_SEX_ID,
        isVaccinated: true,
        isSterilized: true,
        country: "",
        city: "",
        file: null,
        height: 0,
        weight: 0,
    });

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setProfileData((prev) => ({
          ...prev,
          [name]: isNaN(Number(value)) ? value : Number(value),
        }));
    
        if (name === "typeId") props.handleFetchBreeds(Number(value));
      };

    function handleImageUpload(event : React.ChangeEvent<HTMLInputElement>) {
        const {name, files} = event.target;
        const image = files ? files[0]: null;
        if(image) {
            setProfileData(prevState => {
                return({...prevState, [name]: image})
            })
        }
    }

    const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, checked } = e.target;
        setProfileData((prev) => ({
          ...prev,
          [name]: checked,
        }));
    };

    const handleLocationSelect = (city: string, country: string) => {
        setProfileData(prevState => ({
            ...prevState,
            city: city,
            country: country,
        }));
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        props.handleCreateProfile(profileData);
    };

    return(
        <form className="create-profile-form" onSubmit={handleSubmit}>
            <h1 className="create-profile__header">Create profile</h1>
            {props.error && <Error error={props.error}/>}
            <div className="grid">
                <div>
                    <div>
                        <label htmlFor="name">Name</label>
                        <input className="create-profile-form__name" value={profileData.name} placeholder="Pet name" name="name" onChange={handleInputChange} required></input>  
                    </div>

                    <div style={{height:'26%'}}>
                        <label htmlFor="description">About pet</label>
                        <textarea 
                            name="description"
                            defaultValue=''
                            className="create-profile-form__description"
                            value={profileData.description} 
                            placeholder="Tell about your pet" 
                            onChange={handleInputChange}>
                        </textarea>  
                    </div>

                    <div>
                        <label htmlFor="typeId">Animal Type</label>
                        <select name="typeId" className="create-profile-form__animal-type" onChange={handleInputChange} required>
                        {props.animalTypes.map((type) => (
                            <option key={type.id} value={type.id}>
                                {String(type.typeName).charAt(0).toUpperCase() + String(type.typeName).slice(1)}
                            </option>
                        ))}
                        </select>
                    </div>
                    <div>
                        <label htmlFor="breedId">Breed</label>
                        <select name="breedId" className="create-profile-form__breed" onChange={handleInputChange} required>
                            {props.breeds.map((breed) => (
                                <option key={breed.id} value={breed.id}>
                                    {breed.breedName}
                                </option>
                            ))}
                        </select>
                    </div>

                    <LocationSuggestion onSelectLocation={handleLocationSelect}/>
                    <div className="size-input-group">
                        <label htmlFor="height">Height</label>
                        <div className="size-input-container">
                            <span className="unit-label">cm</span>
                            <input name="height" className="height-input" min="30" step="0.5" type="number" value={profileData.height} onChange={handleInputChange}/>
                        </div>
                    </div>

                    <div className="size-input-group">
                        <label htmlFor="weight">Weight</label>
                        <div className="size-input-container">
                            <span className="unit-label">kg</span>
                            <input name="weight" className="weight-input" min="0" step="0.2"  type="number" value={profileData.weight} onChange={handleInputChange}/>
                        </div>
                    </div>
                </div>

                <div>
                    <div>
                        <label htmlFor="dateOfBirth">Date of birth:</label>
                        <input name="dateOfBirth" className="create-profile-form__date-of-birth" type="date" min="2000-01-01" max="2017-04-30" value={profileData.dateOfBirth} onChange={handleInputChange} required/>
                    </div>
                    <div style={{height:'26%'}}>
                        <div>
                            <label htmlFor="sexId">Pet gender:</label>
                            <div className="create-profile-form__gender-buttons-container">
                                {props.sexes.map((sex) => (
                                    <button
                                        key={sex.id}
                                        type="button"
                                        name="sexId"
                                        value={sex.id}
                                        className= {profileData.sexId === sex.id ? "create-profile-form__gender-button selected" : "create-profile-form__gender-button"}
                                        onClick={(e) => handleInputChange(e as any)}>
                                        {sex.sexName}
                                    </button>
                                ))}
                            </div>
                            
                        </div>

                        <div>
                            <FormGroup aria-label="position" row>
                                <FormControlLabel
                                    control={
                                        <CustomSwitch name="isVaccinated" checked={profileData.isVaccinated} onChange={handleCheckboxChange}/>
                                    }
                                    label="Is Vaccinated?"
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
                                        <CustomSwitch name="isSterilized" checked={profileData.isSterilized} onChange={handleCheckboxChange}/>
                                    }
                                    label="Is Sterilized?"
                                    sx={{
                                        fontFamily: "'Karla', sans-serif", 
                                        fontSize: "14px",                  
                                        ".MuiFormControlLabel-label": {
                                          fontFamily: "'Karla', sans-serif", 
                                        },
                                      }}
                                />
                            </FormGroup>
                        </div>
                    </div>

                    <div>
                        <label htmlFor="photo-upload">Upload photo:</label>
                        <FileUploader handleFileUpload={handleImageUpload} maxSizeMB="10" accept="image/*" initialImage={null}/>
                    </div>
                </div>
            </div>
            <br/>
            <LoadButton innertText="Create" loading={props.loading}/>
        </form>
    );
}