import axios, { AxiosResponse } from 'axios';

export interface ProfileData {
    name: string,
    description: string,
    typeId: number,
    breedId: number,
    // files?: File,
    dateOfBirth: string,
    sexId: number,
    isVaccinated: boolean,
    isSterilized: boolean,
    country: string,
    city: string,
    height: number,
    weight: number
}

export async function getAnimalTypes() {
    try {
        const response : AxiosResponse = await axios.get("https://localhost:5295/api/Animal/animal-types");
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Login failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when rectrieving animal types:");
    }
}

export async function getBreeds(animalTypeId: number) {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/Animal/breeds/${animalTypeId}`,);
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Get Brreds failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving breeds:");
    }
}

export async function getSexes() {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/Animal/sexes`,);
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Get Sexes failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving sexes:");
    }
}

export async function createPetProfile(profileData : ProfileData) {
    try {
        const response : AxiosResponse = await axios.post(`https://localhost:5295/api/Animal/animal/profile/create`, profileData, {
            withCredentials: true,
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });

        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Create Animla Profile failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when creating animal profile:");
    }
}

export async function getProfileImage() {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/Animal/animal/image`, {
            withCredentials : true
        });
        if(response.status === 200) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error("Get animal image failed");
        }
    } catch (error) {
        handleError(error, "Error when receiving animal image");
    }
}

function handleError(error : unknown, customMessage: string) {
    if (axios.isAxiosError(error) && error.response) {
        console.log(error)
        const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
        console.error(customMessage, errorMessage);
        throw new Error(errorMessage);
    } else {
        console.error(customMessage);
        throw new Error('An unexpected error occurred.');
    }
}