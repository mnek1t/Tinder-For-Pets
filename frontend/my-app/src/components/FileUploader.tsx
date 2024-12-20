import uploadImage from '../assets/upload-svgrepo-com.svg';
import { useState } from 'react';
import ImagePreview from './ImagePreviewer';
interface FileUploaderProps {
    handleFileUpload : (event : React.ChangeEvent<HTMLInputElement>) => void,
    maxSizeMB: string,
    accept: string,
    initialImage: File | null;
}


const FileUploader = (props : FileUploaderProps) => {
    const [file, setFile] = useState<File | null>(props.initialImage);

    const handleFileUpload = (event : React.ChangeEvent<HTMLInputElement>) => {
        const fileList = event.target.files;
        if (fileList && fileList.length > 0) {
            setFile(fileList[0]);
        } else {
            setFile(null);
        }
        props.handleFileUpload(event);
    }

    return(
        <>
            <div className="upload-file-container">
                {
                    file ? <ImagePreview file={file} /> :
                    <>
                        <img className="upload-image" src={uploadImage} alt='upload'></img>
                        <h3>Click box to upload</h3>
                        <p>Maximun file size {props.maxSizeMB}mb</p>
                    </>
                }
                <input type="file" accept={props.accept} aria-label="Name" name="file" onChange={handleFileUpload}></input>  
            </div>
        </>
        
    );
};
export default FileUploader