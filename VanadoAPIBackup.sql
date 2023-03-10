PGDMP         7                 {         	   VanadoAPI    15.1    15.1                0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16408 	   VanadoAPI    DATABASE     ?   CREATE DATABASE "VanadoAPI" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
    DROP DATABASE "VanadoAPI";
                postgres    false            ?            1259    16420    kvarovi    TABLE     ?  CREATE TABLE public.kvarovi (
    id integer NOT NULL,
    naziv_stroja text NOT NULL,
    naziv text NOT NULL,
    prioritet text NOT NULL,
    vrijeme_pocetka timestamp with time zone NOT NULL,
    vrijeme_zavrsetka timestamp with time zone,
    opis text NOT NULL,
    status text NOT NULL,
    id_stroja integer
);
    DROP TABLE public.kvarovi;
       public         heap    postgres    false            ?            1259    16419    kvarovi_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.kvarovi_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE public.kvarovi_id_seq;
       public          postgres    false    217            	           0    0    kvarovi_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE public.kvarovi_id_seq OWNED BY public.kvarovi.id;
          public          postgres    false    216            ?            1259    16411    strojevi    TABLE     ?   CREATE TABLE public.strojevi (
    id integer NOT NULL,
    naziv text NOT NULL,
    prosjecno_trajanje_kvarova double precision
);
    DROP TABLE public.strojevi;
       public         heap    postgres    false            ?            1259    16410    strojevi_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.strojevi_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 &   DROP SEQUENCE public.strojevi_id_seq;
       public          postgres    false    215            
           0    0    strojevi_id_seq    SEQUENCE OWNED BY     C   ALTER SEQUENCE public.strojevi_id_seq OWNED BY public.strojevi.id;
          public          postgres    false    214            k           2604    16423 
   kvarovi id    DEFAULT     h   ALTER TABLE ONLY public.kvarovi ALTER COLUMN id SET DEFAULT nextval('public.kvarovi_id_seq'::regclass);
 9   ALTER TABLE public.kvarovi ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    216    217    217            j           2604    16414    strojevi id    DEFAULT     j   ALTER TABLE ONLY public.strojevi ALTER COLUMN id SET DEFAULT nextval('public.strojevi_id_seq'::regclass);
 :   ALTER TABLE public.strojevi ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    214    215                      0    16420    kvarovi 
   TABLE DATA           ?   COPY public.kvarovi (id, naziv_stroja, naziv, prioritet, vrijeme_pocetka, vrijeme_zavrsetka, opis, status, id_stroja) FROM stdin;
    public          postgres    false    217   _                  0    16411    strojevi 
   TABLE DATA           I   COPY public.strojevi (id, naziv, prosjecno_trajanje_kvarova) FROM stdin;
    public          postgres    false    215   0                  0    0    kvarovi_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.kvarovi_id_seq', 8, true);
          public          postgres    false    216                       0    0    strojevi_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public.strojevi_id_seq', 3, true);
          public          postgres    false    214            o           2606    16427    kvarovi kvarovi_pkey 
   CONSTRAINT     R   ALTER TABLE ONLY public.kvarovi
    ADD CONSTRAINT kvarovi_pkey PRIMARY KEY (id);
 >   ALTER TABLE ONLY public.kvarovi DROP CONSTRAINT kvarovi_pkey;
       public            postgres    false    217            m           2606    16418    strojevi strojevi_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.strojevi
    ADD CONSTRAINT strojevi_pkey PRIMARY KEY (id);
 @   ALTER TABLE ONLY public.strojevi DROP CONSTRAINT strojevi_pkey;
       public            postgres    false    215            p           2606    16428    kvarovi kvarovi_id_stroja_fkey    FK CONSTRAINT     ?   ALTER TABLE ONLY public.kvarovi
    ADD CONSTRAINT kvarovi_id_stroja_fkey FOREIGN KEY (id_stroja) REFERENCES public.strojevi(id);
 H   ALTER TABLE ONLY public.kvarovi DROP CONSTRAINT kvarovi_id_stroja_fkey;
       public          postgres    false    3181    215    217               ?   x?}?M? F?p??M	姜?MU?`+4?x㽤IM????%??f$??jo?M?Vw??iχ?!&?mH???zxKYKEC?m? ?????4?;?
?s??;???????I?)?"7?ɜC?Vʕ?Q??¾?}Te??4?{V?z???G?."?5?f{C*	?J8?o?????Sr$??"p?          <   x?3?,.(J????VH?/?,K?4?2?L??OI??LN???3???02?407? ?=... ??     