import axios, { AxiosResponse } from 'axios';
import Cookies from 'js-cookie';

export interface ProfileData {
    name: string,
    description: string,
    typeId: number,
    breedId: number,
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
        const response : AxiosResponse = await axios.get("https://localhost:5295/api/AnimalProfile/animal-types");
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Login failed.'); 
        }
    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error when rectrieving animal types:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error when rectrieving animal types:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function getBreeds(animalTypeId: number) {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/AnimalProfile/breeds/${animalTypeId}`,);
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Get Brreds failed.'); 
        }
    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error when retrieving breeds:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error when retrieving breeds:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function getSexes() {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/AnimalProfile/sexes`,);
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Get Sexes failed.'); 
        }
    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error when retrieving sexes:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error when retrieving sexes:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}

export async function createPetProfile(profileData : ProfileData) {
    try {
        const token = Cookies.get('AuthToken');
        console.log('Token', token)
        console.log('API CALL', profileData);
        const response : AxiosResponse = await axios.post(`https://localhost:5295/api/AnimalProfile/animal/profile/create`, profileData, {
            withCredentials: true,
            headers: {
                'Authorization': `Bearer ${token}`, // if your API requires it
                'Content-Type': 'application/json',
            }
        });
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Get Sexes failed.'); 
        }
    } catch (error) {
        if (axios.isAxiosError(error) && error.response) {
            console.log(error)
            const errorMessage = error.response.data.errors?.[0]?.description || 'An unexpected error occurred.';
            console.error('Error when creating profile:', errorMessage);
            throw new Error(errorMessage);
        } else {
            console.error('Error when creating profile:',);
            throw new Error('An unexpected error occurred.');
        }
    }
}