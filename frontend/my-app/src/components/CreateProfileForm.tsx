import "../styles/form.css";
import { getAnimalTypes, getBreeds, getSexes } from "../api/profileApi";
import React, { useState, useEffect } from "react";
import LocationSuggestion from "./LocationSuggestion"; 
import {ProfileData} from "../api/profileApi";


interface CreateProfileProps {
    createProfile : (event: React.FormEvent<HTMLFormElement>, data : ProfileData) => void
}

interface AnimalType {
    id: number,
    typeName: string;
}

interface Breed {
    id: number,
    breedName: string;
}

interface Sex {
    id: number,
    sexName: string;
}

export default function CreateProfileForm(props : CreateProfileProps) {
    document.body.style.background = '#FEFAF6';
    const [animalTypes, setAnimalTypes] = useState<JSX.Element[]>([]);
    const [breeds, setBreeds] = useState<JSX.Element[]>([]);
    const [sexes, setSexes] = useState<JSX.Element[]>([]);
    const [selectedSex, setSelectedSex] = useState<number>(1);
    const [selectedAnimalType, setSelectedAnimalType] = useState<number>(1);
    const [selectedBreed, setSelectedBreed] = useState<number>(98);
    const [profileData, setProfileData] = useState<ProfileData>({
        name: "",
        description: "",
        typeId: selectedAnimalType,
        breedId: selectedBreed,

        dateOfBirth: "",
        sexId: selectedSex,
        isVaccinated: true,
        isSterilized: true,
        country: "",
        city: "",
        height: 0,
        weight: 0
    });

    
   
    useEffect(() => {
        getAnimalTypes()
        .then((data) => {
            const elements = data.map((type: AnimalType) => {
                return(
                    <option className="animal-type" key={type.id} value={type.id}>
                         {type.typeName.charAt(0).toUpperCase() + type.typeName.slice(1)}
                    </option>
                );
            })
            setAnimalTypes(elements);
        }).catch(error => {
            console.error(error);
        });
    }, [])

    useEffect(() => {
        getSexes()
        .then((data) => {
            const elements = data.map((sex : Sex) => {
                return(
                    <button name="sexId" key={sex.id} value={sex.id} className={`gender-button ${selectedSex === sex.id ? "selected" : ""}`} onClick={handleClick}>
                        {sex.sexName}
                    </button>
                );
            });

            setSexes(elements);
        })
    }, [selectedSex])

    useEffect(() => {
        getBreeds(selectedAnimalType)
        .then((data) => {
            const elements = data.map((breed: Breed) => {
                return(
                    <option className="breed" key={breed.id} value={breed.id}>
                         {breed.breedName.charAt(0).toUpperCase() + breed.breedName.slice(1)}
                    </option>
                );
            })
            setBreeds(elements);
        }).catch(error => {
            console.error(error);
        })
    }, [selectedAnimalType]);

    function handleInput(event : React.ChangeEvent<HTMLInputElement> | React.ChangeEvent<HTMLSelectElement> | React.ChangeEvent<HTMLTextAreaElement>) {
        const {name, value} = event.target;
        if(name === 'typeId') {
            console.log('type id works')
            setSelectedAnimalType(Number(value));
            setSelectedBreed(Number(value) === 1 ? 98 : 1);
            setProfileData(prevState => {
                return {...prevState, "breedId": Number(value) === 1 ? 98 : 1}
            })
        }

        if(name === 'breedId') {
            setSelectedBreed(Number(value));
        }

        if(isNaN(Number(value))) {
            setProfileData(prevState => {
                return {...prevState, [name]: value}
            })
        }
        else {
            setProfileData(prevState => {
                return {...prevState, [name]: Number(value)}
            })
        }
    }

    function handleClick(event : React.MouseEvent<HTMLButtonElement>) {
        event.preventDefault();
        const {name, value} = event.target as HTMLButtonElement;
        if (name === "sexId") {
            setSelectedSex(Number(value));
        }

        setProfileData(prevState => {
            return {...prevState, [name]: Number(value)}
        })
        
    }

    const handleLocationSelect = (city: string, country: string) => {
        setProfileData(prevState => ({
            ...prevState,
            city: city,
            country: country,
        }));
    };

    const handleCheck = (event : React.ChangeEvent<HTMLInputElement>) => {
        const {name, checked} = event.target;

        setProfileData(prevState => {
            return {...prevState, [name]: checked}
        })
    };

    return(
        <form className="create-profile-form" onSubmit={(e) => props.createProfile(e, profileData)}>
            <h1 className="create-profile__header">Create profile</h1>
            <div className="grid">
                <div>
                    <div>
                        <label htmlFor="name-input">Name</label>
                        <input value={profileData.name} id="name-input" placeholder="Pet name" name="name" onChange={handleInput} required></input>  
                    </div>
                    <div>
                        <label htmlFor="name-input">About pet</label>
                        <textarea id="description-input" name="description" value={profileData.description} placeholder="Tell about your pet" onChange={handleInput}></textarea>  
                    </div>
                    <div>
                        <label htmlFor="animal-type-dropbox">Animal Type</label>
                        <select name="typeId" id="animal-type-dropbox" onChange={handleInput}>
                            {animalTypes}
                        </select>
                    </div>
                    <div>
                        <label htmlFor="breeds-dropbox">Breed</label>
                        <select name="breedId" id="breeds-dropbox" onChange={handleInput}>
                            {breeds}
                        </select>
                    </div>
                    <div>
                        <label htmlFor="photo-upload">Upload photo</label>
                        <input type="file" accept="image/*" aria-label="Name" id="photo-upload" placeholder="Pet name" name="name" onChange={handleInput}></input>  
                    </div>

                </div>
                <div>
                    <div>
                        <label htmlFor="date-of-birth-dropbox">Date of birth</label>
                        <input id="date-of-birth-dropbox" type="date" name="dateOfBirth" min="2000-01-01" max="2017-04-30" value={profileData.dateOfBirth} onChange={handleInput}/>
                    </div>

                    <div>
                        <label>Select a pet gender:</label>
                        {sexes}
                    </div>

                    <div>
                        <span className="checkbox-inputs">
                            <span className="checkbox-group">
                                <label>Is Vaccinated?</label>
                                <input type="checkbox" name="isVaccinated" checked={profileData.isVaccinated} onChange={handleCheck}/>
                            </span>
                            <span className="checkbox-group">
                                <label>Is Sterilized?</label>
                                <input type="checkbox" name="isSterilized" checked={profileData.isSterilized} onChange={handleCheck}/>
                            </span>
                        </span>
                    </div>

                    <div>
                        <LocationSuggestion onSelectLocation={handleLocationSelect}/>
                    </div>
                   
                    <div className="size-input-group">
                        <label htmlFor="date-of-birth-dropbox">Height</label>
                        <div className="size-input-container">
                            <span className="unit-label">cm</span>
                            <input id="date-of-birth-dropbox" className="height-input" min="30" step="0.5" name="height" type="number" value={profileData.height} onChange={handleInput}/>
                        </div>
                    </div>

                    <div className="size-input-group">
                        <label htmlFor="date-of-birth-dropbox">Weight</label>
                        <div className="size-input-container">
                            <span className="unit-label">kg</span>
                            <input id="date-of-birth-dropbox" className="weight-input" min="0" step="0.2" name="weight" type="number" value={profileData.weight} onChange={handleInput}/>
                        </div>
                    </div>

                    <div>
                        <button type="submit">Create</button>
                    </div>
                </div>
            </div>
        </form>
    );
}