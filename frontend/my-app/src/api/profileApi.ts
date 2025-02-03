import axios, { AxiosResponse } from 'axios';

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
    file: File | null,
    height: number,
    weight: number
}

export interface ProfileDetailsData {
    animal : {
        id: string,
        type?: string,
        breed?: string,
    }
    profile : {
        id: string
        name: string,
        description?: string,
        age: number,
        sex?: string,
        dateOfBirth?:string,
        isVaccinated?: boolean,
        isSterilized?: boolean,
        city?: string,
        country?: string
    }
    images : {
        imageData: string,
        imageFormat : string
    }[]
}

export type MatchCardProfileData =  {
    matchId: string,
    profileName: string,
    isVaccinated: boolean,
    description: string,
    isSterilized: boolean,
    age: number,
    createdAt: string,
    images : {
        imageData: string,
        imageFormat : string
    }[]
}

export interface AnimalMediaUploadRequest {
    description: string,
    file: File
}

export interface UpdateAnimalProfileRequest {
    name : string,
    typeId : number,
    description : string,
    dateOfBirth : string,
    sexId : number,
    isVaccinated : boolean,
    isSterilized : boolean,
    breedId : number,
    city : string | undefined,
    country : string | undefined,
    //height : number,
    //weight : number
}


export async function getMatches() {
    try {
        const response : AxiosResponse<MatchCardProfileData[]> = await axios.get(`https://localhost:5295/Match`, {
            withCredentials : true
        });
        if(response.status === 200) {
            console.log(response.data)
            return response.data;
        } else {
            throw new Error('Error when retrieving matches failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving matches:");
        return [];
    }
}

export async function createSwipe(profileId: string, isLike: boolean) {
    try {
        const response : AxiosResponse = await axios.post("https://localhost:5295/api/swipe/save", {
            petSwipedOnProfileId: profileId,
            isLike: isLike
        }, {
            withCredentials : true,
        });
        if(response.status === 201) {
            console.log(response.data)
            return response.data;
        } else {
            throw new Error('Error when creating a swipe.'); 
        }
    } catch (error) {
        handleError(error, "Error when creating a swipe.");
    }
}

export async function getProfileRecommendations() : Promise<ProfileDetailsData[]> {
    try {
        const response : AxiosResponse<ProfileDetailsData[]> = await axios.get("https://localhost:5295/recommendations", {
            withCredentials : true,
            params : {
                radiusKm : 12
            }
        });
        if(response.status === 200) {
            console.log(response.data)
            return response.data;
        } else {
            throw new Error('Error when retrieving recommendations failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving recommendations:");
        return [];
    }
}

export async function getAnimalTypes() {
    try {
        const response : AxiosResponse = await axios.get("https://localhost:5295/api/Animal/animal-types");
        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Error when retrieving animal types failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving animal types:");
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

export async function getProfileDetails() {
    try {
        const response : AxiosResponse = await axios.get(`https://localhost:5295/api/Animal/animal/data`, {
            withCredentials: true
        });
        if(response.status === 200) {
            console.log('getProfileDetails',response.data)
            return response.data;
        } else {
            throw new Error('Get Sexes failed.'); 
        }
    } catch (error) {
        handleError(error, "Error when retrieving sexes:");
    }
}

export async function createPetProfile(profileData : ProfileData) {
    console.log('profileData: ', profileData)
    try {
        const formData = new FormData();
        formData.append("Name", profileData.name);
        formData.append("Description", profileData.description);
        formData.append("AnimalTypeId", String(profileData.typeId));
        formData.append("BreedId", String(profileData.breedId));
        formData.append("DateOfBirth", profileData.dateOfBirth);
        formData.append("SexId", String(profileData.sexId));
        formData.append("IsVaccinated", String(profileData.isVaccinated));
        formData.append("IsSterilized", String(profileData.isSterilized));
        formData.append("Country", profileData.country);
        formData.append("City", profileData.city);
        formData.append("Height", String(profileData.height));
        formData.append("Weight",  String(profileData.weight));

        if (profileData.file) {
            formData.append("File", profileData.file);
        }
        console.log(formData.get("AnimalTypeId"))
        const response : AxiosResponse = await axios.post(`https://localhost:5295/api/Animal/animal/profile/create`, formData, {
            withCredentials: true,
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });

        if(response.status === 200) {
            return response.data;
        } else {
            throw new Error('Create Animal Profile failed.'); 
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

export async function uploadProfileImage(requestData: AnimalMediaUploadRequest) {
    try {
        const formData = new FormData();
        formData.append("Description", requestData.description);
        formData.append("File", requestData.file);

        const response : AxiosResponse = await axios.post(`https://localhost:5295/api/Animal/animal/image/upload`, formData, {
            withCredentials : true,
            headers : {
                "Content-Type": "multipart/form-data"
            }
        });
        if(response.status === 204) {
            console.log(response.data);
            return response.data;
        } else {
            throw new Error("Update Profile Image failed");
        }
    } catch (error) {
        handleError(error, "Error when updating animal image");
    }
}

export async function updatePetProfile(profileId: string, updateRequestData: UpdateAnimalProfileRequest) {
    try {
        console.log('updateRequestData', updateRequestData)
        const response : AxiosResponse = await axios.post(`https://localhost:5295/api/Animal/animal/profile/update/${profileId}`, updateRequestData, {
            withCredentials : true,
        });
        if(response.status === 204) {
            console.log(response.data)
            return response.data;
        } else {
            throw new Error('Error when updating profile.'); 
        }
    } catch (error) {
        handleError(error, "Error when updating profile.");
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