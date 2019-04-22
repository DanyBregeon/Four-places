# Four-places

Ce qui est fait :
	- Un écran affichant la liste des lieux (représenté par leur image, leur nom et le début de leur description) triée par distance par rapport à notre position actuelle
	- Un écran de détail d'un lieu avec sa position sur google map, sa description complète et ses commentaires; on peut ajouter un commentaire si on est connecté
	- Un écran d'inscription
	- Un écran de connexion
	- Quand on est connecté, un écran permettant de visualiser son profil
	- Quand on est connecté, un écran permettant d'éditer son profil (avec l'option de choisir une image de profil depuis la galerie ou en prenant une photo)
	- Quand on est connecté, un écran permettant de changer son mot de passe
	- Quand on est connecté, un écran permettant d'ajouter un lieu (avec l'option de choisir une image depuis la galerie ou en prenant une photo) et qui utilise comme position par défaut notre position actuelle
	- Quand on est connecté, un bouton pour se déconnecter
	
Ce qui reste à faire :
	- Gestion du cache
	- Gérer le cas où l'utilisateur n'est pas connecté à internet
	
Problèmes connus :
	- A l'ajout d'un lieu ou lors de la modification du profil si on a changé d'image de profil, il faut attendre plusieurs secondes avant que l'ajout/la modification soit validé
	- Problème de fluidité lorque l'on scroll rapidemment la liste des lieux