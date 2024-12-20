import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import CreateProfileForm from "../components/profile/CreateProfileForm"
import EmptyHeader from "../components/EmptyHeader";
import { createPetProfile, getAnimalTypes, getBreeds, getSexes, ProfileData } from "../api/profileApi";
import { DEFAULT_ANIMAL_TYPE_ID } from "../utils/TinderConstants";
import { InvalidFormatError } from '../utils/CustomErrors';

export default function CreateProfilePage() {
    document.body.style.background = '#FEFAF6';
    const [animalTypes, setAnimalTypes] = useState([]);
    const [breeds, setBreeds] = useState([]);
    const [sexes, setSexes] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<Error | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        getAnimalTypes()
          .then((animalTypes) => {
            setAnimalTypes(animalTypes);
            handleFetchBreeds(animalTypes[0]?.id || DEFAULT_ANIMAL_TYPE_ID);
        })
        .catch((err) => setError(err));
    }, []);

    useEffect(() => {
        getSexes()
          .then(setSexes)
          .catch((err) => setError(err));
    }, []);

    const handleFetchBreeds = (typeId: number) => {
        getBreeds(typeId)
          .then(setBreeds)
          .catch((err) => setError(err));
    };

    function handleCreatePetProfile(perProfileData: ProfileData) {
        setLoading(true);
        createPetProfile(perProfileData)
        .then(() => {
            navigate("/app/profile");
            setError(null);
        })
        .catch((error: Error) => {
           setError(error);
        })
        .finally(() => {
            setLoading(false);
        });
    }

    return(
        <>
            <EmptyHeader/>
            <CreateProfileForm 
                handleCreateProfile={handleCreatePetProfile} 
                error={error}  
                loading={loading}
                animalTypes={animalTypes}
                sexes={sexes}
                breeds={breeds}
                handleFetchBreeds={handleFetchBreeds}/>
        </>
    );
}