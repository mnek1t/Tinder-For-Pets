interface AuthorCardProps {
    name: string;
    description: string;
    imageSrc: string;
}
export default function AuthorCard(props: AuthorCardProps) {
    return(
    <>
        <div className="author-card-wrapper">
          <h3>{props.name}</h3>
          <img src={props.imageSrc} alt='author'></img>
          <p>{props.description}</p>
        </div>
    </>);
}